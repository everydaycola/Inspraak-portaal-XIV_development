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
    public IEnumerable<PlanningGroupMember> GetAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId);

    public Dictionary<string, int> CalculateCrossDistributionAbsolute(Guid panelId);

    //ADD
    public Panel AddPanel(string name, double sampleRate,
        ICollection<Criteria> distribution, int citizenCount, double reservePercentage,
        double responseRate, string userId);

    public void AddTextPost(Guid panelId,string title, string content, bool isVisibleForPanelMembers);
    public void AddPlanningsGroupMember(Guid panelId, string Email, string Naam, string Functie);

    public void AddDocumentPost(Guid panelId, string title, string documentUrl, bool isVisibleForPanelMembers);
    //UPDATE
    public void UpdatePanel(Guid id, bool isRegistrationOpen);
    public void UpdatePanelRegistrationCount(Guid id, bool increase);
    public PanelMember UpdatePanelMember(PanelMember member);
    //DELETE
    public void DeletePlanningsGroupmember(Guid planningsGroupMemberId);
    //HELPERS
    public void NewPanelPhase(Guid guid, double newResponseRate);
    public void EndRegistration(Guid id, IEnumerable<Criteria> allDesiredCriteriaPercentages, bool sendInvitationMails, string currentBaseUrl);
}