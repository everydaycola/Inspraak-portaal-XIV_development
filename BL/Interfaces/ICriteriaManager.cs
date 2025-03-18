using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    public IEnumerable<Criteria> GetAllCriteriaWithValuesForPanel(Guid panelId);
    public IEnumerable<Criteria> GetAllDefaultCriteriaWithValuesForPanel(Guid panelId);
    public IEnumerable<CriteriaGroup> GetAllCriteriaGroupForPanel(Guid panelId);
    public CriteriaGroup GetCriteriaGroupByPanelIdAndName(Guid panelId, string groupName);

    public CriteriaGroup AssignMemberToCriteriaGroup(Guid panelId, Dictionary<string, string> CriteriaAnswers,
        PanelMember member);
}