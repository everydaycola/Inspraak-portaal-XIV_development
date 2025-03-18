using System.Text.RegularExpressions;
using BL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class CalculationManager : ICalculationManager
{
    private ICriteriaManager _critManager;
    public CalculationManager(ICriteriaManager critManager)
    {
        _critManager = critManager;
    }

    public double CalculateTotalMemberCount(Guid panelId)
    {
        var criteriaGroups = _critManager.GetAllCriteriaGroupForPanel(panelId);
        var totalMembers = criteriaGroups.SelectMany(group => group.PanelMembers).Distinct().Count();
        return totalMembers;
    }
    public Dictionary<string, double> CalculateAllCriteriaCountForPanel(Guid panelId)
    {
        Dictionary<string, double> critCountMap = new Dictionary<string, double>();
        var allCriteria = _critManager.GetAllCriteriaWithValuesForPanel(panelId);
        foreach (var crit in allCriteria)
        {
            foreach (var val in crit.Values)
            {
                critCountMap.Add(crit.Name + "-" + val.Value, CalculateAmountOfMembersWithSpecificCriteria(crit.Panel.Id, crit.Name, val.Value));
            }
        }

        return critCountMap;
    }
    public double CalculateAmountOfMembersWithSpecificCriteria(Guid panelId, string searchedCriteriaName, string searchedCriteriaValue)
    {
        var criteriaGroups = _critManager.GetAllCriteriaGroupForPanel(panelId);
        var matchingGroups = criteriaGroups.Where(group =>
            group.CriteriaAnswers.Any(a =>
                a.criteria.Name.Equals(searchedCriteriaName) == true &&
                a.criteriaValue.Value.Equals(searchedCriteriaValue) == true));
        var allMatchingMembers = matchingGroups
            .SelectMany(group => group.PanelMembers);
        return allMatchingMembers.Count();
    }
}