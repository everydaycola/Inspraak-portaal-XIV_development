using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    //READ
    public Panel ReadAllCriteriaWithValuesForPanel(Guid panelId);
    public Dictionary<string, IEnumerable<string>> ReadAllCriteriaNamesAndOptions(Guid panelId);
    public Dictionary<string, Dictionary<string, int>> ReadAllCriteriaMemberCountsWithValuesForPanel(Guid panelId);
    public Criteria ReadCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
}