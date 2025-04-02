using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    //GETS
    public Panel GetAllCriteriaWithValuesForPanel(Guid panelId);
    public IEnumerable<Criteria> GetAllNonDefaultCriteriaWithValuesForPanel(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponses(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>>
        GetPanelMembersWithCompletedCriteriaGroupedByResponse(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponsesForDefaultCriteria(
        Guid panelId);
    public Criteria GetCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
    
    //SAVES
    void SavePanelMemberCriteriaResponses(Guid panelId,Dictionary<string, string> CriteriaAnswers, PanelMember member);
}