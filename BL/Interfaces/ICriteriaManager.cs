using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    public Panel GetAllCriteriaWithValuesForPanel(Guid panelId);
    public IEnumerable<Criteria> GetAllNonDefaultCriteriaWithValuesForPanel(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponses(Guid panelId);
    //public IEnumerable<CriteriaGroup> GetAllCriteriaGroupForPanel(Guid panelId);
    //public CriteriaGroup GetCriteriaGroupByMemberId(Guid memberId);
    //public CriteriaGroup GetCriteriaGroupById(Guid criteriaGroupId);
    //public CriteriaGroup AssignMemberToCriteriaGroup(Guid panelId, Dictionary<string, string> CriteriaAnswers,PanelMember member);
}