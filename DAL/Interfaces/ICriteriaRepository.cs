using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    public IEnumerable<Criteria> ReadAllCriteriaWithValuesForPanel(Guid panelId);
    public IEnumerable<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId);
    CriteriaGroup ReadCriteraGroupByPanelIdAndName(Guid panelId, string groupName);
    IEnumerable<Criteria> ReadAllDefaultCriteriaWithValuesForPanel(Guid panelId);
}