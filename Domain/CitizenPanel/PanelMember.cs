namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public Panel Panel { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }
    public bool HasAnsweredAllQuestions { get; set; }
    
    //This field would later be moved into Identity.
    public string Email { get; set; }

    //EMPTY CONSTRUCTOR FOR EF
    public PanelMember()
    {
    }

    public PanelMember(Panel panel)
    {
        Panel = panel;
        HasAnsweredAllQuestions = false;
    }
    public PanelMember(Panel panel, CriteriaGroup criteriaGroup)
    {
        Panel = panel;
        CriteriaGroup = criteriaGroup;
        HasAnsweredAllQuestions = false;
    }

}