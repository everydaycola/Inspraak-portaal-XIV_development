using BL;
using DAL;
using DAL.EF;
using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

//CODE TOT TEST ENTITYFRAMEWORK CONNECTION.
/*var optionsBuilder = new DbContextOptionsBuilder<CitizenPanelDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Database=CitizenPanel_DB;Username=user;Password=password;");
CitizenPanelDbContext cpdc = new CitizenPanelDbContext(optionsBuilder.Options);
PanelRepository pr = new PanelRepository(cpdc);
PanelManager pm = new PanelManager(pr);

cpdc.CreateDatabase(false);

Panel panel = new Panel("TestPanel");
pm.addPanel(panel);
*/