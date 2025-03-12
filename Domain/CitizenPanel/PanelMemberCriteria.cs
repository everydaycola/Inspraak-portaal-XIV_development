namespace Domain.CitizenPanel;

public class PanelMemberCriteria
{
    public PanelMember PanelMember { get; set; }
    public Criteria Criteria { get; set; }

    public PanelMemberCriteria(PanelMember panelMember, Criteria criteria)
    {
        PanelMember = panelMember;
        Criteria = criteria;
    }

    // empty constructor for EF
    public PanelMemberCriteria()
    {
    }
}