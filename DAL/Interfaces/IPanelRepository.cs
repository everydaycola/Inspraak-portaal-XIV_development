using System.Collections;
using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface IPanelRepository
{
    //READ
    public Panel ReadPanel(Guid id);
    public IEnumerable<Panel> ReadAllPanels();
    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid id);
    public PanelMember ReadPanelMemberWithPanel(Guid id);
    public Panel ReadPanelWithRepresentationGroup(Guid id);
    public IEnumerable<PanelMember> ReadPanelMembersWithCriteria(Guid id);
    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId);
    public IEnumerable<PlanningGroupMember> ReadAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId);

    public IEnumerable<PanelMember> ReadPanelMembersWithResponses(Guid id);
    //UPDATE
    public void UpdatePanel(Panel panel);
    public void UpdatePanelMember(PanelMember member);

    public void UpdatePanelMembersToSelected(ICollection<PanelMember> selectedMembers);
    //CREATE
    public void CreatePanelMember(PanelMember panelMember);
    //DELETE
    public void RemoveAllUnselectedPanelmembers(Guid panelId);
}