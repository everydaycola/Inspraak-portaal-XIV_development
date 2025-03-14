namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public Panel Panel { get; set; }
    public CriteriaGroup CriteriaGroup { get; set; }

    //EMPTY CONSTRUCTOR FOR EF
    public PanelMember()
    {
    }

    public PanelMember(Panel panel)
    {
        Panel = panel;
    }
    public PanelMember(Panel panel, CriteriaGroup criteriaGroup)
    {
        Panel = panel;
        CriteriaGroup = criteriaGroup;
    }

}