namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public ICollection<PanelMemberCriteria> PanelMembers { get; set; }

    public Criteria(string name, string value)
    {
        this.Name = name;
        this.Value = value;
        this.PanelMembers = new List<PanelMemberCriteria>();
    }
}
