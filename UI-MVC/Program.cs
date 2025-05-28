using BL.Generator;
using BL.Interfaces;
using BL.Managers;
using BL.Options;
using DAL.EF;
using DAL.Interfaces;
using DAL.Repositories;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UI_MVC;
using UI_MVC.Models;
using UI_MVC.TempTenant;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
builder.Services.AddScoped<IOrganisationManager, OrganisationManager>();
builder.Services.AddScoped<ICustomUserManager, CustomUserManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPanelRepository, PanelRepository>();
builder.Services.AddScoped<IPanelManager, PanelManager>();
builder.Services.AddScoped<IPanelProjectPageManager, PanelProjectPageManager>();
builder.Services.AddScoped<QrCodeGenerator, QrCodeGenerator>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<ICriteriaManager, CriteriaManager>();
builder.Services.AddScoped<ICriteriaRepository, CriteriaRepository>();
builder.Services.AddScoped<ICalculationManager, CalculationManager>();
builder.Services.AddScoped<ISendMailManager, SendMailManager>();
builder.Services.AddScoped<IStorageManager, StorageManager>();
builder.Services.AddScoped<IPinCRepository, PinCRepository>();
builder.Services.AddScoped<ICommuneManager, CommuneManager>();
builder.Services.AddScoped<IPanelProjectPageManager, PanelProjectPageManager>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionManager, QuestionManager>();

//Tenant specific logic
builder.Services
    .AddOrganisationContext()
    .AddScoped<OrganisationMiddleware>();

builder.Services.Configure<GoogleCloudOptions>(options =>
{
    options.BucketName = Environment.GetEnvironmentVariable("GoogleCloud_BucketName") ??
                         builder.Configuration.GetValue<string>("GoogleCloud_BucketName");
});
var redisConfiguration = builder.Configuration.GetValue<string>("Redis_Configuration");
var redisInstanceName = builder.Configuration.GetValue<string>("Redis_InstanceName");
var redis = ConnectionMultiplexer.Connect(redisConfiguration);
builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConfiguration;
    options.InstanceName = redisInstanceName;
});


var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
                       builder.Configuration.GetValue<string>("ConnectionStrings__DefaultConnection");
builder.Services.AddDbContext<CitizenPanelDbContext>(options => { options.UseNpgsql(connectionString); });


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services
    .AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<CitizenPanelDbContext>()
    .AddUserStore<ApplicationUserStore>()
    .AddSignInManager<MultiOrganisationSignInManager>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CitizenPanelDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CitizenPanelDbContext>();
    if (context.CreateDatabase(dropDatabase: true))
    {
        // Identity
        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        IdentitySeeder identitySeeder = new IdentitySeeder(userManager, roleManager);
        await identitySeeder.SeedAsync();

        DataSeeder.Seed(context);
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<OrganisationMiddleware>();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program
{
}; //Nodig om de config binnen tests te kunnen gebruiken.