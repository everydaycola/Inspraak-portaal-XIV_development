using BL;
using DAL;
using DAL.EF;
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

PanelRepository pr = new PanelRepository(cpdc);
PanelManager pm = new PanelManager(pr);
ConsoleUi consoleUi = new ConsoleUi(pm);
consoleUi.Start();



