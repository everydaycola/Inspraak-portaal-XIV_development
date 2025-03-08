namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public Panel Panel { get; set; }
    public ICollection<PanelMemberCriteria> Criteria { get; set; }

    //EMPTY CONSTRUCTOR FOR EF
    public PanelMember()
    {
    }

    public PanelMember(Panel panel)
    {
        Panel = panel;
    }
}