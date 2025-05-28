using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    // READ
    public Dictionary<string, Dictionary<string, int>> ReadAllCriteriaMemberCountsWithValuesForPanel(Guid panelId, bool onlyUnknown = false);
    public IEnumerable<Criteria> ReadAllCriteriaForPanelWithAnswerOptions(Guid panelId, bool onlyDefault = false, bool includeKnown = true, bool includeUnknown = true);
    public Criteria ReadCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
    List<PanelMember> ReadAllRegisteredPanelMembersOfPanel(Guid panelId);
    
    // CREATE
    
    // UPDATE
    
    // DELETE
}