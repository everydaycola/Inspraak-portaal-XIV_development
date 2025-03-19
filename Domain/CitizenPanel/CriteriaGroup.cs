namespace Domain.CitizenPanel;

public class CriteriaGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsADefaultGroup { get; set; }
    public ICollection<CriteriaAnswer> CriteriaAnswers { get; set; }
    public ICollection<PanelMember> PanelMembers { get; set; }

    public CriteriaGroup()
    {
        CriteriaAnswers = new List<CriteriaAnswer>();
        PanelMembers = new List<PanelMember>();
    }

    public CriteriaGroup(string name, List<PanelMember> panelMembers, bool isADefaultGroup)
    {
        Name = name;
        CriteriaAnswers = new List<CriteriaAnswer>();
        PanelMembers = panelMembers;
        IsADefaultGroup = isADefaultGroup;
    }
}