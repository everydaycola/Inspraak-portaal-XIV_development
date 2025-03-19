namespace Domain.CitizenPanel;

public class CriteriaAnswer
{
    public Guid Id { get; set; }
    public Criteria Criteria { get; set; }
    public CriteriaValue CriteriaValue { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    public CriteriaAnswer()
    {
    }

    public CriteriaAnswer(Criteria criteria, CriteriaValue criteriaValue)
    {
        this.Criteria = criteria;
        this.CriteriaValue = criteriaValue;
    }
}