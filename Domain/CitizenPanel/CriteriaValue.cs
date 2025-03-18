namespace Domain.CitizenPanel;

public class CriteriaValue
{
    public Guid CriteriaValueId { get; set; }
    public string Value { get; set; }
    public Criteria criteria { get; set; }
    public double distributionPercentage { get; set; }
    public ICollection<CriteriaAnswer> CriteriaAnswers { get; set; }

    public CriteriaValue(string value, double distributionPercentage)
    {
        Value = value;
        this.distributionPercentage = distributionPercentage;
    }
}