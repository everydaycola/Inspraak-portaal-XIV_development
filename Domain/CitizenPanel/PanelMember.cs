namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public bool HasAnsweredAllQuestions { get; set; }
    public string Email { get; set; }
    public Panel Panel { get; set; }
    public ICollection<CriteriaResponse> Responses { get; set; }
    //public CriteriaGroup CriteriaGroup { get; set; }

}