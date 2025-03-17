using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class CriteriaManager : ICriteriaManager
{
    private readonly ICriteriaRepository _repo;

    public CriteriaManager(ICriteriaRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Criteria> GetAllCriteriaWithValuesForPanel(Guid panelId)
    {
        return _repo.ReadAllCriteriaWithValuesForPanel(panelId);
    }

    public IEnumerable<Criteria> GetAllDefaultCriteriaWithValuesForPanel(Guid panelId)
    {
        return _repo.ReadAllDefaultCriteriaWithValuesForPanel(panelId);
    }

    public IEnumerable<CriteriaGroup> GetAllCriteriaGroupForPanel(Guid panelId)
    {
        return _repo.ReadAllCriteriaGroupForPanel(panelId);
    }

    public CriteriaGroup GetCriteriaGroupByPanelIdAndName(Guid panelId, string groupName)
    {
        return _repo.ReadCriteraGroupByPanelIdAndName(panelId, groupName);
    }
    
}
