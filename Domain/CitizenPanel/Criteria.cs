namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public Panel Panel { get; set; }
    public string Name { get; set; }
    public string Question { get; set; }
    public ICollection<CriteriaValue> Values { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    public Criteria(string name)
    {
        this.Name = name;
        Values = new List<CriteriaValue>();
    }
    public Criteria(string name, string question)
    {
        this.Name = name;
        Values = new List<CriteriaValue>();
        this.Question = question;
    }
    public Criteria(string name, string question,CriteriaGroup criteriaGroup)
    {
        this.Name = name;
        this.Question = question;
        CriteriaGroup = CriteriaGroup;
        Values = new List<CriteriaValue>();
    }
}
