using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface IPanelRepository
{
    public Panel ReadPanel(Guid id);
    public PanelMember ReadPanelMember(Guid id);
    public PanelMember ReadPanelMemberWithPanel(Guid id);
    public Panel ReadPanelWithRepresentationGroup(Guid id);
    public IEnumerable<PanelMember> ReadPanelMembersWithCriteria(Guid id);
    public IEnumerable<Panel> ReadAllPanels();
    public PanelMember ReadPanelByUserId(Guid memberId);
    public void UpdatePanel(Panel panel);
    public PanelMember UpdatePanelMember(PanelMember member);
    public void CreatePanel(Panel panel);
    public void CreatePanelMember(PanelMember panelMember);
    public void DeletePanel(Panel panel);
    public void DeletePanelMember(PanelMember member);
    PanelMember ReadPanelWithMembersAndRepresentationGroup(Guid id);
    void CreateCriteria(Criteria criteria);
}