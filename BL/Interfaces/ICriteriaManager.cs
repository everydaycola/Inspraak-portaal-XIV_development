using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    public ICollection<CriteriaGroup> GetAllCriteriaGroupForPanel(Guid panelId);
    public CriteriaGroup GetCriteriaGroupByPanelIdAndName(Guid panelId, string groupName);
}