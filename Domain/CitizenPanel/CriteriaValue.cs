namespace Domain.CitizenPanel;

public class CriteriaValue
{
    public Guid CriteriaValueId { get; set; }
    public string Value { get; set; }
    public Criteria Criteria { get; set; }
    public double DistributionPercentage { get; set; }

    public CriteriaValue(string value, double distributionPercentage)
    {
        Value = value;
        DistributionPercentage = distributionPercentage;
    }
}