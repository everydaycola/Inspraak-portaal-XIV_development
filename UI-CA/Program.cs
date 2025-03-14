using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using UI_CA;

//CODE TOT TEST ENTITYFRAMEWORK CONNECTION.
var optionsBuilder = new DbContextOptionsBuilder<CitizenPanelDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Database=CitizenPanel_DB;Username=user;Password=password;");
var cpdc = new CitizenPanelDbContext(optionsBuilder.Options);

// for dev work?
if (cpdc.CreateDatabase(dropDatabase: true))
{
    DataSeeder.Seed(cpdc);
}

// // for real work? 
// cpdc.CreateDatabase(false);

IPanelRepository pr = new PanelRepository(cpdc);
IPanelManager pm = new PanelManager(pr);
ConsoleUi consoleUi = new ConsoleUi(pm);
consoleUi.Start();



