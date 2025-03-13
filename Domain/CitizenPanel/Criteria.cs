namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public double distributionPercentage { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    public Criteria(string name, string value,double distributionPercentage)
    {
        this.Name = name;
        this.Value = value;
        this.distributionPercentage = distributionPercentage;
    }
    public Criteria(string name, string value, CriteriaGroup criteriaGroup)
    {
        this.Name = name;
        this.Value = value;
        CriteriaGroup = CriteriaGroup;
    }
}
