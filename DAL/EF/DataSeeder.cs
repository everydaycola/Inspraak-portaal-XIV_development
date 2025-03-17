using Domain.CitizenPanel;

namespace DAL.EF;

public static class DataSeeder
{
   private static CitizenPanelDbContext _context;
    public static void Seed(CitizenPanelDbContext context)
    {
        Console.WriteLine("Seeding...");
        _context = context;
        //REPRESENTATION GROUPS
        var rg1 = new RepresentationGroup(20000, 0.2, 0.1);
        //PANELS
        var panel1 = new Panel("Verkeersveiligheid in en rond Antwerpen.", 0.005);
        
        //PanelMembers
        var panelMember1 = new PanelMember(panel1);
        var panelMember2 = new PanelMember(panel1);
        var panelMember3 = new PanelMember(panel1);
        var panelMember4 = new PanelMember(panel1);
        var panelMember5 = new PanelMember(panel1);
        
        //CriteriaGroup
        var criteriaGroup1 = new CriteriaGroup("Rijbewijs-fiets",new List<PanelMember>{panelMember1,panelMember5});
        var criteriaGroup2 = new CriteriaGroup("Rijbewijs-Auto",new List<PanelMember>{panelMember2});
        var criteriaGroup3 = new CriteriaGroup("Rijbewijs-TeVoet",new List<PanelMember>{panelMember3}); 
        var criteriaGroup4 = new CriteriaGroup("GeenRijbewijs-TeVoet",new List<PanelMember>{panelMember4});
        
        
        //CRITERIA
        var criteria1 = new Criteria("Rijbewijs");
        var criteria2 = new Criteria("Vervoermethode");
        
        //CRITERIA VALUES
        var value1 = new CriteriaValue("Ja", 0.5);
        var value2 = new CriteriaValue("Nee", 0.5);
        var value3 = new CriteriaValue("Te voet", 0.33);
        var value4 = new CriteriaValue("Fiets", 0.33);
        var value5 = new CriteriaValue("Auto", 0.33);
        
        //BIND CRITERIA WITH VALUES
        criteria1.Values.Add(value1);
        criteria1.Values.Add(value2);
        criteria2.Values.Add(value3);
        criteria2.Values.Add(value4);
        criteria2.Values.Add(value5);
        
        //Add criteria to criteriagroups
        criteriaGroup1.Criteria.Add(criteria1);
        criteriaGroup1.Criteria.Add(criteria2);
        
        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        //SET REGISTRATION TO OPEN
        panel1.IsRegistrationOpen = true;
        
        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        
        AddMultipleEntities([panelMember1, panelMember2, panelMember3, panelMember4]);
        AddMultipleEntities([criteria1, criteria2]);
        AddMultipleEntities([criteriaGroup1, criteriaGroup2, criteriaGroup3, criteriaGroup4]);
        AddMultipleEntities([value1, value2, value3, value4, value5]);
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
    
    private static void AddMultipleEntities<T>(List<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            _context.Set<T>().Add(entity); // Using DbSet<T>.Add from the DbContext
        }
    }
}