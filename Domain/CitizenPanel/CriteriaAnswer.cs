namespace Domain.CitizenPanel;

public class CriteriaAnswer
{
    public Guid Id { get; set; }
    public Criteria criteria { get; set; }
    public CriteriaValue criteriaValue { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    public CriteriaAnswer()
    {
    }

    public CriteriaAnswer(Criteria criteria, CriteriaValue criteriaValue)
    {
        this.criteria = criteria;
        this.criteriaValue = criteriaValue;
    }
}