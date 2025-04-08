using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IPanelManager
{
    //GET
    public Panel GetPanel(Guid id);
    IEnumerable<Panel> GetAllPanelsOwnedBy(string userId);
    public Panel GetPanelWithRepresentationGroup(Guid id);
    public PanelMember GetPanelMemberWithCriteriaResponses(Guid id);
    public PanelMember GetPanelMemberWithPanel(Guid id);
    IEnumerable<PanelMember> GetAllPanelMembersForPanel(Guid panelId);
    public IEnumerable<PanelMember> GetAllPanelMembersWhichAnsweredAllQuestionsWithCriteria(Guid id);
    Panel GetPanelWithCriteriaAndCriteriaAnswerOptions(Guid panelId);
    //ADD
    public Panel AddPanel(string name, int size, double sampleRate,
        Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage,
        double responseRate, string userId);
    //UPDATE
    public void UpdatePanel(Guid id, bool isRegistrationOpen);
    public void UpdatePanelRegistrationCount(Guid id, bool increase);
    public PanelMember UpdatePanelMember(PanelMember member);
    public IEnumerable<PlanningGroupMember> GetAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId);
}