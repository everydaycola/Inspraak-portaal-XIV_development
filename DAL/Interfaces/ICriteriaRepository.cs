using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface ICriteriaRepository
{
    IEnumerable<Criteria> ReadAllCriteriaWithValuesForPanel(Guid panelId);
    IEnumerable<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId);
    CriteriaGroup ReadCriteriaGroupForPanel(Guid panelId, string groupName);
    CriteriaGroup ReadCriteraGroupByPanelIdAndName(Guid panelId, string groupName);
    IEnumerable<Criteria> ReadAllNonDefaultCriteriaWithValuesForPanel(Guid panelId);
    IEnumerable<Criteria> ReadAllDefaultCriteriaWithValuesForPanel(Guid panelId);
    void UpdateCriteriaGroup(CriteriaGroup criteriaGroup);
    void CreateCriteriaGroup(CriteriaGroup newCriteriaGroup);


    CriteriaValue ReadCriteriaValueBasedOnCriteriaAndValue(Guid criteriaId, string criteriaValue);
    Criteria ReadCriteriaByName(Guid panelId,string critName);
}