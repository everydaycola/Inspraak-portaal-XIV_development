using Domain.CitizenPanel;

namespace DAL.EF;

public static class DataSeeder
{
    public static void Seed(CitizenPanelDbContext context)
    {
        //REPRESENTATION GROUPS
        RepresentationGroup rg1 = new RepresentationGroup(20000, 0.2, 0.1);
        //PANELS
        Panel panel1 = new Panel("Verkeersveiligheid in en rond Antwerpen.", 0.005);

        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}