using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    //GETS
    public Panel GetAllCriteriaWithValuesForPanel(Guid panelId);
    public Dictionary<string, Dictionary<string, int>> GetAllCriteriaCountsGroupedByValue(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>>
        GetPanelMembersWithCompletedCriteriaGroupedByResponse(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponsesForDefaultCriteria(
        Guid panelId);
    
    //SAVES
    void SavePanelMemberCriteriaResponses(Guid panelId,Dictionary<string, string> CriteriaAnswers, PanelMember member);
}