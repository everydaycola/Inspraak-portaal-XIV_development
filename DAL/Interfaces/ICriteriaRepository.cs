using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    //READ
    public Dictionary<string, IEnumerable<string>> ReadAllCriteriaNamesAndOptions(Guid panelId);
    public Dictionary<string, Dictionary<string, int>> ReadAllCriteriaMemberCountsWithValuesForPanel(Guid panelId);
    public Dictionary<string, Dictionary<string, double>> ReadAllDesiredCriteriaPercentages(Guid panelId);
    public Criteria ReadCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
}