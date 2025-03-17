namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public Panel Panel { get; set; }
    public string Name { get; set; }
    public string Question { get; set; }
    public ICollection<CriteriaValue> Values { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }
    public bool IsDefault { get; set; }

    public Criteria(string name, bool isDefault)
    {
        this.Name = name;
        Values = new List<CriteriaValue>();
        IsDefault = isDefault;
    }
    public Criteria(string name,string question,bool isDefault)
    {
        this.Name = name;
        Values = new List<CriteriaValue>();
        this.Question = question;
        IsDefault = isDefault;
    }
}
