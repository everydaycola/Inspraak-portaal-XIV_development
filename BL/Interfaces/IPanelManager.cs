using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IPanelManager
{
    //GET
    public Panel GetPanel(Guid id);
    public IEnumerable<Panel> GetAllPanels();
    public Panel GetPanelWithRepresentationGroup(Guid id);
    public PanelMember GetPanelMemberWithCriteriaResponses(Guid id);
    public PanelMember GetPanelMemberWithPanel(Guid id);
    public IEnumerable<PanelMember> GetAllPanelMembersForPanel(Guid panelId);
    public Panel GetPanelWithCriteriaAndCriteriaAnswerOptions(Guid panelId);

    public Panel GetPanelWithPosts(Guid panelId);

    //ADD
    public Panel AddPanel(string name, int size, double sampleRate,
        ICollection<Criteria> distribution, int citizenCount, double reservePercentage,
        double responseRate, string userId);

    public void AddTextPost(Guid panelId,string title, string content);

    //UPDATE
    public void UpdatePanel(Guid id, bool isRegistrationOpen);
    public void UpdatePanelRegistrationCount(Guid id, bool increase);
    public PanelMember UpdatePanelMember(PanelMember member);
    public IEnumerable<PlanningGroupMember> GetAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId);

    public void AddDocumentPost(Guid panelId,string title, string documentUrl);

    //HELPERS
    public void NewPanelPhase(Guid guid, double newResponseRate);
    public void EndRegistration(Guid id, Dictionary<string, Dictionary<string, double>> allDesiredCriteriaPercentages);
}