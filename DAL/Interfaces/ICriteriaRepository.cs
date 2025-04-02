using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    //READ
    Panel ReadAllCriteriaWithValuesForPanel(Guid panelId);
    IEnumerable<Criteria> ReadAllNonDefaultCriteriaWithValuesForPanel(Guid panelId);
    Criteria ReadCriteriaByName(Guid panelId,string critName);
    public Criteria ReadCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
}