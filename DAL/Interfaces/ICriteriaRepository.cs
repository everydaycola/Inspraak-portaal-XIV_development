using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    public ICollection<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId);
    CriteriaGroup ReadCriteraGroupByPanelIdAndName(Guid panelId, string groupName);
}