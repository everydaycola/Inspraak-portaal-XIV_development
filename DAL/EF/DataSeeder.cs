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
        Criteria criteria1 = new Criteria("Rijbewijs","Yes",0.5);
        Criteria criteria2 = new Criteria("Rijbewijs","No",0.5);
        Criteria criteria3 = new Criteria("Vervoermethode","Te voet",1/3);
        Criteria criteria4 = new Criteria("Vervoermethode","Fiets",1/3);
        Criteria criteria5 = new Criteria("Vervoermethode", "Auto",1/3);
        
        //Add criteria to criteriagroups
        criteriaGroup1.Criteria.Add(criteria1);
        criteriaGroup1.Criteria.Add(criteria4);
        criteriaGroup2.Criteria.Add(criteria1);
        criteriaGroup2.Criteria.Add(criteria5);
        criteriaGroup3.Criteria.Add(criteria1);
        criteriaGroup3.Criteria.Add(criteria3);
        criteriaGroup4.Criteria.Add(criteria2);
        criteriaGroup4.Criteria.Add(criteria3);
        
        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        
        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        addMultiplePanelMembers(new List<PanelMember>() { panelMember1, panelMember2, panelMember3, panelMember4 });
        addMultipleCriteria(new List<Criteria>() { criteria1, criteria2, criteria3, criteria4, criteria5 });
        addMultipleCriteriaGroups(new List<CriteriaGroup>()
        { criteriaGroup1, criteriaGroup2, criteriaGroup3, criteriaGroup4 });
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
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