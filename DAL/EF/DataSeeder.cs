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
        
        //CRITERIA
        Criteria criteria1 = new Criteria("Rijbewijs","Yes");
        Criteria criteria2 = new Criteria("Rijbewijs","No");
        Criteria criteria3 = new Criteria("Vervoermethode","Te voet");
        Criteria criteria4 = new Criteria("Vervoermethode","Fiets");
        Criteria criteria5 = new Criteria("Vervoermethode","Auto");
        
        //PanelMembers
        PanelMember PanelMember1 = new PanelMember(panel1);
        PanelMember PanelMember2 = new PanelMember(panel1);
        PanelMember PanelMember3 = new PanelMember(panel1);
        PanelMember PanelMember4 = new PanelMember(panel1);
        
        //PanelMemberCriteria
        PanelMemberCriteria panelMemberCriteria1 = new PanelMemberCriteria(PanelMember1, criteria1);
        PanelMemberCriteria panelMemberCriteria2 = new PanelMemberCriteria(PanelMember1, criteria4);
        PanelMemberCriteria panelMemberCriteria3 = new PanelMemberCriteria(PanelMember2, criteria1);
        PanelMemberCriteria panelMemberCriteria4 = new PanelMemberCriteria(PanelMember2, criteria5);
        PanelMemberCriteria panelMemberCriteria5 = new PanelMemberCriteria(PanelMember3, criteria1);
        PanelMemberCriteria panelMemberCriteria6 = new PanelMemberCriteria(PanelMember3, criteria3);
        PanelMemberCriteria panelMemberCriteria7 = new PanelMemberCriteria(PanelMember4, criteria2);
        PanelMemberCriteria panelMemberCriteria8 = new PanelMemberCriteria(PanelMember4, criteria3);
        
        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        addMultiplePanelMembers(new List<PanelMember>() { PanelMember1, PanelMember2, PanelMember3, PanelMember4 });
        addMultipleCriteria(new List<Criteria>() { criteria1, criteria2, criteria3, criteria4, criteria5 });
        addMultiplePanelMemberCriteria(new List<PanelMemberCriteria>()
        {
            panelMemberCriteria1, panelMemberCriteria2, panelMemberCriteria3, panelMemberCriteria4,
            panelMemberCriteria5, panelMemberCriteria6, panelMemberCriteria7, panelMemberCriteria8
        });
        
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
    private static void addMultiplePanelMemberCriteria(List<PanelMemberCriteria> panelMemberCriteria)
    {
        foreach (var pmc in panelMemberCriteria)
        {
            _context.PanelMemberCriteria.Add(pmc);
        }
    }
    
}