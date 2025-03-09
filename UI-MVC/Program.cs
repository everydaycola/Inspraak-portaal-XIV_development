using BL;
using DAL;
using DAL.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRepository, PanelRepository>();
builder.Services.AddScoped<IManager, PanelManager>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CitizenPanelDbContext>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CitizenPanelDbContext>();

    if (context.CreateDatabase(dropDatabase: true))
    {
        DataSeeder.Seed(context);
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();