namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public Panel Panel { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }
    public bool hasAnsweredAllQuestions { get; set; }
    
    //This field would later be moved into Identity.
    public string Email { get; set; }

    //EMPTY CONSTRUCTOR FOR EF
    public PanelMember()
    {
    }

    public PanelMember(Panel panel)
    {
        Panel = panel;
        hasAnsweredAllQuestions = false;
    }
    public PanelMember(Panel panel, CriteriaGroup criteriaGroup)
    {
        Panel = panel;
        CriteriaGroup = criteriaGroup;
        hasAnsweredAllQuestions = false;
    }

}