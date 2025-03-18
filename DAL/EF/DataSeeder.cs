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
        
        //CriteriaGroup (default groups SHOULD be based on default values only)!
        CriteriaGroup criteriaGroup1 = new CriteriaGroup("Man",new List<PanelMember>{panelMember1,panelMember3,panelMember5});
        CriteriaGroup criteriaGroup2 = new CriteriaGroup("Vrouw",new List<PanelMember>{panelMember2, panelMember4});
        
        
        //CRITERIA
        Criteria criteria3 = new Criteria("Geslacht", "Wat is uw geslacht?", true);
        Criteria criteria1 = new Criteria("Rijbewijs","Beschikt u over een rijbewijs?",false);
        Criteria criteria2 = new Criteria("Vervoermethode","Wat is uw voorkeurs vervoersmethode?", false);
       
        
        //CRITERIA VALUES
        CriteriaValue value1 = new CriteriaValue("Ja", 0.5);
        CriteriaValue value2 = new CriteriaValue("Nee", 0.5);
        CriteriaValue value3 = new CriteriaValue("Te voet", 0.33);
        CriteriaValue value4 = new CriteriaValue("Fiets", 0.33);
        CriteriaValue value5 = new CriteriaValue("Auto", 0.33);
        CriteriaValue value6 = new CriteriaValue("Man", 0.5);
        CriteriaValue value7 = new CriteriaValue("Vrouw", 0.5);
        
        //BIND CRITERIA WITH VALUES
        criteria1.Values.Add(value1);
        criteria1.Values.Add(value2);
        criteria2.Values.Add(value3);
        criteria2.Values.Add(value4);
        criteria2.Values.Add(value5);
        criteria3.Values.Add(value6);
        criteria3.Values.Add(value7);
        
        //Add criteria to criteriagroups
        criteriaGroup1.CriteriaAnswers.Add(new CriteriaAnswer()
        {
            criteria = criteria1,
            criteriaValue = value6
        });
        criteriaGroup2.CriteriaAnswers.Add(new CriteriaAnswer()
        {
            criteria = criteria1,
            criteriaValue = value7
        });
        
        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        //LINK CRITERIA WITH PANEL
        panel1.PanelCriteria = new List<Criteria>() { criteria1, criteria2 , criteria3};
        
        //SET REGISTRATION TO OPEN
        panel1.IsRegistrationOpen = true;
        
        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        
        addMultiplePanelMembers(new List<PanelMember>() { panelMember1, panelMember2, panelMember3, panelMember4 });
        addMultipleCriteria(new List<Criteria>() { criteria1, criteria2, criteria3 });
        addMultipleCriteriaGroups(new List<CriteriaGroup>()
            { criteriaGroup1, criteriaGroup2 });
        addMultipleCriteriaValue(new List<CriteriaValue>() { value1, value2, value3, value4, value5, value6, value7 });
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