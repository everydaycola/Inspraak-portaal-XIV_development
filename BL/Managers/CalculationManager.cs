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

    public int CalculateSuccesfulRegistrationCount(Guid panelId)
    {
        return _critManager.GetAllCriteriaGroupForPanel(panelId)
            .SelectMany(group => group.PanelMembers)
            .Where(member => member.HasAnsweredAllQuestions)
            .Distinct()
            .Count();
    }
    public Dictionary<string, Dictionary<string, int>> CalculateAllCriteriaCountForPanel(Guid panelId)
    {
        var allCriteria = _critManager.GetAllCriteriaWithValuesForPanel(panelId);
        // dictionary with criteria name -> criteria value <-> amount of members with this criteria
        return allCriteria.ToDictionary(
            crit => crit.Name,
            crit => crit.Values.ToDictionary(
                val => val.Value,
                val => CalculateAmountOfMembersWithSpecificCriteria(crit.Panel.Id, crit.Name, val.Value)
            ));
    }
    public int CalculateAmountOfMembersWithSpecificCriteria(Guid panelId, string searchedCriteriaName, string searchedCriteriaValue)
    {
        var criteriaGroups = _critManager.GetAllCriteriaGroupForPanel(panelId);
        var matchingGroups = criteriaGroups.Where(group =>
            group.CriteriaAnswers.Any(a =>
                a.Criteria.Name.Equals(searchedCriteriaName) == true &&
                a.CriteriaValue.Value.Equals(searchedCriteriaValue) == true));
        var allMatchingMembers = matchingGroups
            .SelectMany(group => group.PanelMembers);
        return allMatchingMembers.Count();
    }
}