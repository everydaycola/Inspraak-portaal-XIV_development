using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IPanelManager
{
    //GET
    public Panel GetPanel(Guid id);
    public IEnumerable<Panel> GetAllPanels();
    IEnumerable<Panel> GetAllPanelsOwnedBy(string userId);
    public Panel GetPanelWithRepresentationGroup(Guid id);
    public ICollection<PanelMember> GetPanelMembersAndRepresentationGroup(Guid id);
    public IEnumerable<PanelMember> GetPanelWithPanelMembersAndCriteria(Guid id);
    public PanelMember GetPanelMemberById(Guid memberId);
    public PanelMember GetPanelMemberWithCriteriaResponses(Guid id);
    public PanelMember GetPanelMemberWithPanel(Guid id);
    IEnumerable<PanelMember> GetAllPanelMembersForPanel(Guid panelId);
    public IEnumerable<PanelMember> GetAllPanelMembersWhichAnsweredAllQuestionsWithCriteria(Guid id);
    Panel GetPanelWithCriteriaAndCriteriaAnswerOptions(Guid panelId);

    Panel GetPanelWithPosts(Guid panelId);
    //ADD
    public Panel AddPanel(string name, int size, double sampleRate,
        ICollection<Criteria> distribution, int citizenCount, double reservePercentage,
        double responseRate, string userId);
    void AddTextPost(Guid panelId, string content);
    //UPDATE
    public void UpdatePanel(Guid id, bool isRegistrationOpen);
    public void UpdatePanelRegistrationCount(Guid id, bool increase);
    public PanelMember UpdatePanelMember(PanelMember member);
    public IEnumerable<PlanningGroupMember> GetAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId);
}