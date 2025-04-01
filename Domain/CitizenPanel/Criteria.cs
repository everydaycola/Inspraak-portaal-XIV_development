namespace Domain.CitizenPanel;

public class Criteria
{
    public Guid CriteriaId { get; set; }
    public string Name { get; set; }
    public string Question { get; set; }
    public bool IsDefault { get; set; }
    public ICollection<CriteriaAnswerOption> AnswerOptions { get; set; }
    //public ICollection<CriteriaValue> Values { get; set; }
    //public ICollection<CriteriaAnswer> CriteriaAnswers { get; set; }
    //public CriteriaGroup CriteriaGroup { get; set; }
}
