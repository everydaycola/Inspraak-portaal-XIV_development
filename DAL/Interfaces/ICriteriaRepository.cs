using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    IEnumerable<Criteria> ReadAllCriteriaWithValuesForPanel(Guid panelId);
    IEnumerable<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId);
    CriteriaGroup ReadCriteriaGroupByMemberId(Guid memberId);
    CriteriaGroup ReadCriteriaGroupForPanel(Guid panelId, string groupName);
    CriteriaGroup ReadCriteriaGroupByid(Guid criteriaGroupId);
    IEnumerable<Criteria> ReadAllNonDefaultCriteriaWithValuesForPanel(Guid panelId);
    IEnumerable<Criteria> ReadAllDefaultCriteriaWithValuesAndAnswerForPanel(Guid panelId);
    void UpdateCriteriaGroup(CriteriaGroup criteriaGroup);
    void CreateCriteriaGroup(CriteriaGroup newCriteriaGroup);


    CriteriaValue ReadCriteriaValueBasedOnCriteriaAndValue(Guid criteriaId, string criteriaValue);
    Criteria ReadCriteriaByName(Guid panelId,string critName);
}