namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    public Criteria(string name, string value)
    {
        this.Name = name;
        this.Value = value;
    }
    public Criteria(string name, string value, CriteriaGroup criteriaGroup)
    {
        this.Name = name;
        this.Value = value;
        CriteriaGroup = CriteriaGroup;
    }
}
