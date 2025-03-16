namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public string Name { get; set; }
    public ICollection<CriteriaValue> Values { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    public Criteria(string name)
    {
        this.Name = name;
        Values = new List<CriteriaValue>();
    }
    public Criteria(string name, CriteriaGroup criteriaGroup)
    {
        this.Name = name;
        CriteriaGroup = CriteriaGroup;
        Values = new List<CriteriaValue>();
    }
}
