namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid Id { get; set; }
    public Panel Panel { get; set; }
    // public ICollection<Criteria> Criteria { get; set; }

    public PanelMember(Panel panel)
    {
        Panel = panel;
    }
}