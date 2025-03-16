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
        RepresentationGroup rg1 = new RepresentationGroup(20000, 0.2, 0.1);
        //PANELS
        Panel panel1 = new Panel("Verkeersveiligheid in en rond Antwerpen.", 0.005);
        
        //PanelMembers
        PanelMember panelMember1 = new PanelMember(panel1);
        PanelMember panelMember2 = new PanelMember(panel1);
        PanelMember panelMember3 = new PanelMember(panel1);
        PanelMember panelMember4 = new PanelMember(panel1);
        PanelMember panelMember5 = new PanelMember(panel1);
        
        //CriteriaGroup
        CriteriaGroup criteriaGroup1 = new CriteriaGroup("Rijbewijs-fiets",new List<PanelMember>{panelMember1,panelMember5});
        CriteriaGroup criteriaGroup2 = new CriteriaGroup("Rijbewijs-Auto",new List<PanelMember>{panelMember2});
        CriteriaGroup criteriaGroup3 = new CriteriaGroup("Rijbewijs-TeVoet",new List<PanelMember>{panelMember3}); 
        CriteriaGroup criteriaGroup4 = new CriteriaGroup("GeenRijbewijs-TeVoet",new List<PanelMember>{panelMember4});
        
        
        //CRITERIA
        Criteria criteria1 = new Criteria("Rijbewijs");
        Criteria criteria2 = new Criteria("Vervoermethode");
        
        //CRITERIA VALUES
        CriteriaValue value1 = new CriteriaValue("Ja", 0.5);
        CriteriaValue value2 = new CriteriaValue("Nee", 0.5);
        CriteriaValue value3 = new CriteriaValue("Te voet", 0.33);
        CriteriaValue value4 = new CriteriaValue("Fiets", 0.33);
        CriteriaValue value5 = new CriteriaValue("Auto", 0.33);
        
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
        
        addMultiplePanelMembers(new List<PanelMember>() { panelMember1, panelMember2, panelMember3, panelMember4 });
        addMultipleCriteria(new List<Criteria>() { criteria1, criteria2 });
        addMultipleCriteriaGroups(new List<CriteriaGroup>()
            { criteriaGroup1, criteriaGroup2, criteriaGroup3, criteriaGroup4 });
        addMultipleCriteriaValue(new List<CriteriaValue>() { value1, value2, value3, value4, value5 });
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
    
    private static void addMultipleCriteriaValue(List<CriteriaValue> values)
    {
        foreach (var val in values)
        {
            _context.CriteriaValues.Add(val);
        }
    }
    
    private static void addMultiplePanelMembers(List<PanelMember> members)
    {
        foreach (var mem in members)
        {
            _context.PanelMembers.Add(mem);
        }
    }
    private static void addMultipleCriteria(List<Criteria> criteria)
    {
        foreach (var crit in criteria)
        {
            _context.Criteria.Add(crit);
        }
    }
    private static void addMultipleCriteriaGroups(List<CriteriaGroup> criteriaGroups)
    {
        foreach (var cg in criteriaGroups)
        {
            _context.CriteriaGroups.Add(cg);
        }
    }


}