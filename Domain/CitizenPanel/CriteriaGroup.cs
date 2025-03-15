namespace Domain.CitizenPanel;

public class CriteriaGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Criteria> Criteria { get; set; }
    public ICollection<PanelMember> PanelMembers { get; set; }

    public CriteriaGroup()
    {
        Criteria = new List<Criteria>();
        PanelMembers = new List<PanelMember>();
    }

    public CriteriaGroup(string name, List<PanelMember> panelMembers)
    {
        Name = name;
        Criteria = new List<Criteria>();
        PanelMembers = panelMembers;
    }
    
}