namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid Id { get; set; }
    public Panel Panel { get; set; }

    //EMPTY CONSTRUCTOR FOR EF
    public PanelMember()
    {
    }

    public PanelMember(Panel panel)
    {
        Panel = panel;
    }
}