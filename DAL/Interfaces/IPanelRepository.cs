using System.Collections;
using Domain.CitizenPanel;
using Domain.Interfaces;
using Domain.Interfaces.Posts;

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
    public Panel ReadPanelWithPosts(Guid panelId);
    public PlanningGroupMember ReadPlanningGroupMember(Guid planningsGroupMemberId);

    //UPDATE
    public void UpdatePanel(Panel panel);
    public void UpdatePanelMember(PanelMember member);

    public void UpdatePanelMembersToSelected(ICollection<PanelMember> selectedMembers);
    //CREATE
    public void CreatePanelMember(PanelMember panelMember);
    public void CreatePanelMembers(List<PanelMember> panelMembers);
    public void CreateTextPost(Guid panelId, TextPost textPost);
    public void CreateDocumentPost(Guid panelId, DocumentPost docPost);
    void CreateMeetingPost(Guid panelId, MeetingPost meetingPost);
    public void CreatePlanningsGroupMember(PlanningGroupMember member);
    
    //REMOVE
    public void RemoveAllUnselectedPanelmembers(Guid panelId);
    public void RemovePlanningGroupMember(Guid planningsGroupMemberId);
    public void AddSummaryToMeetingPost(Guid meetingId, string uniqueFileName);
}