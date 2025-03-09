using Domain.CitizenPanel;

namespace DAL.EF;

public static class DataSeeder
{
    public static void Seed(CitizenPanelDbContext context)
    {
        context.Panels.Add(new Panel("verkeersveiligheid in en rond Antwerpen."));
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}