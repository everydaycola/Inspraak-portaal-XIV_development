using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    //READ
    public Dictionary<string, Dictionary<string, int>> ReadAllCriteriaMemberCountsWithValuesForPanel(Guid panelId);
    public IEnumerable<Criteria> ReadAllCriteriaForPanelWithAnswerOptions(Guid panelId, bool onlyDefault = false);
    public Criteria ReadCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
}