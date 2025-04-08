using BL.Generator;
using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UI_MVC;

var builder = WebApplication.CreateBuilder(args);
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPanelRepository, PanelRepository>();
builder.Services.AddScoped<IPanelManager, PanelManager>();
builder.Services.AddScoped<QrCodeGenerator, QrCodeGenerator>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<ICriteriaManager, CriteriaManager>();
builder.Services.AddScoped<ICriteriaRepository, CriteriaRepository>();
builder.Services.AddScoped<ICalculationManager, CalculationManager>();

var redisConfiguration = builder.Configuration.GetValue<string>("Redis:Configuration");
var redisInstanceName = builder.Configuration.GetValue<string>("Redis:InstanceName");

logger.LogInformation($"Redis Configuration: {redisConfiguration}");
logger.LogInformation($"Redis Instance Name: {redisInstanceName}");

try
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConfiguration;
        options.InstanceName = redisInstanceName;
    });
    logger.LogInformation("Redis cache configured successfully.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Error configuring Redis cache.");
}
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
builder.Services.AddDbContext<CitizenPanelDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddSession(options =>
{
    logger.LogInformation($"Session Cookie HttpOnly: {options.Cookie.HttpOnly}");
    logger.LogInformation($"Session Cookie IsEssential: {options.Cookie.IsEssential}");
    logger.LogInformation($"Session Cookie IdleTimeout: {options.IdleTimeout}");

    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services
    .AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CitizenPanelDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CitizenPanelDbContext>();
    if (context.CreateDatabase(dropDatabase: true))
    {
        // Identity
        var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        IdentitySeeder identitySeeder = new IdentitySeeder(userManager, roleManager);
        await identitySeeder.SeedAsync();

        DataSeeder.Seed(context);
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    var sessionId = context.Session.Id;
    logger.LogInformation($"Request Session ID: {sessionId}");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
// todo: the /id thing isnt very relevant here, copied from .net project. 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program
{
}; //Nodig om de config binnen tests te kunnen gebruiken.