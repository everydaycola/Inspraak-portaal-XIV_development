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
        var panel1 = new Panel
        {
            Name="Verkeersveiligheid in en rond Antwerpen.", 
            SampleRate = 0.005
        };
        
        //PanelMembers
        var panelMember1 = new PanelMember { Panel = panel1 };
        var panelMember2 = new PanelMember{ Panel = panel1 };
        var panelMember3 = new PanelMember{ Panel = panel1 };
        var panelMember4 = new PanelMember{ Panel = panel1 };
        var panelMember5 = new PanelMember{ Panel = panel1 };
        
        //CriteriaGroup (default groups SHOULD be based on default values only)!
        //TODO: REMOVE THESE LINES
        //var criteriaGroup1 = new CriteriaGroup("Man",new List<PanelMember>{panelMember1,panelMember3,panelMember5}, true);
        //var criteriaGroup2 = new CriteriaGroup("Vrouw",new List<PanelMember>{panelMember2, panelMember4}, true);
        
        //CRITERIA
        var criteria1 = new Criteria
        {
            Name="Rijbewijs",
            Question="Beschikt u over een rijbewijs?",
            IsDefault = false
            
        };
        var criteria2 = new Criteria
        {
            Name="Vervoermethode",
            Question="Wat is uw voorkeurs vervoersmethode?", 
            IsDefault = false
        };
        var criteria3 = new Criteria
        {
            Name="Geslacht",
            Question="Wat is uw geslacht?",
            IsDefault=true
        };
        //criteriaAnswerOptions
        var cao1 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Ja"
        };
        var cao2 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Nee"
        };
        var cao3 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.33,
            Option = "Te voet"
        };
        var cao4 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.33,
            Option = "Fiets"
        };
        var cao5 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.33,
            Option = "Auto"
        };
        var cao6 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Man"
        };
        var cao7 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Vrouw"
        };
        //CRITERIA VALUES
        //TODO REMOVE THESE
        /*
        var value1 = new CriteriaValue("Ja", 0.5);
        var value2 = new CriteriaValue("Nee", 0.5);
        var value3 = new CriteriaValue("Te voet", 0.33);
        var value4 = new CriteriaValue("Fiets", 0.33);
        var value5 = new CriteriaValue("Auto", 0.33);
        var value6 = new CriteriaValue("Man", 0.5);
        var value7 = new CriteriaValue("Vrouw", 0.5);
        
        //BIND CRITERIA WITH VALUES
        criteria1.Values.Add(value1);
        criteria1.Values.Add(value2);
        criteria2.Values.Add(value3);
        criteria2.Values.Add(value4);
        criteria2.Values.Add(value5);
        criteria3.Values.Add(value6);
        criteria3.Values.Add(value7);
        */
        //Add criteria to criteriagroups
        /*criteriaGroup1.CriteriaAnswers.Add(new CriteriaAnswer()
        {
            Criteria = criteria3,
            CriteriaValue = value6
        });
        criteriaGroup2.CriteriaAnswers.Add(new CriteriaAnswer()
        {
            Criteria = criteria3,
            CriteriaValue = value7
        });*/
        
        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        //LINK CRITERIA WITH PANEL
        panel1.PanelCriteria = new List<Criteria>() { criteria1, criteria2 , criteria3};
        
        //LINK ANSWER OPTIONS WITH CRITERIA
        criteria1.AnswerOptions.Add(cao1);
        criteria1.AnswerOptions.Add(cao2);
        criteria2.AnswerOptions.Add(cao3);
        criteria2.AnswerOptions.Add(cao4);
        criteria2.AnswerOptions.Add(cao5);
        criteria3.AnswerOptions.Add(cao6);
        criteria3.AnswerOptions.Add(cao7);
        
        //SET REGISTRATION TO OPEN
        panel1.IsRegistrationOpen = true;
        
        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        
        AddMultipleEntities([panelMember1, panelMember2, panelMember3, panelMember4, panelMember5]);
        AddMultipleEntities([criteria1, criteria2, criteria3]);
        //AddMultipleEntities([criteriaGroup1, criteriaGroup2 ]);
        //AddMultipleEntities([value1, value2, value3, value4, value5, value6, value7 ]);
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