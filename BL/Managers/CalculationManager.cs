using System.Text.RegularExpressions;
using BL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class CalculationManager : ICalculationManager
{
    private ICriteriaManager _critManager;
    private readonly IPanelManager _panelManager;

    public CalculationManager(ICriteriaManager critManager, IPanelManager panelManager)
    {
        _critManager = critManager;
        _panelManager = panelManager;
    }
    public int CalculateSuccesfulRegistrationCount(Guid panelId)
    {
        return _panelManager.GetAllPanelMembersForPanel(panelId)
            .Where(m => m.HasAnsweredAllQuestions)
            .Distinct()
            .Count();
    }
    public Dictionary<string, Dictionary<string, int>> CalculateAllCriteriaCountForPanel(Guid panelId)
    {
        var allCriteria = _critManager.GetAllCriteriaWithValuesForPanel(panelId).Criteria;
    
        return allCriteria.ToDictionary(
            crit => crit.Name,
            crit => crit.AnswerOptions.ToDictionary(
                answer => answer.Option,
                answer => CalculateAmountOfMembersWithSpecificCriteria(
                    panelId,     
                    crit.Name, 
                    answer.Option
                )
            ));
    }
    private int CalculateAmountOfMembersWithSpecificCriteria(Guid panelId, string searchedCriteriaName, string searchedCriteriaValue)
    {
        var responseGroups = _critManager.GetPanelMembersGroupedByResponses(panelId);
        return responseGroups
            .Where(group => GroupContainsCriteria(group.Key, searchedCriteriaName, searchedCriteriaValue))
            .SelectMany(group => group.Value)
            .Count(panelMember => panelMember.HasAnsweredAllQuestions);
    }

    private bool GroupContainsCriteria(string groupKey, string criteriaName, string criteriaValue)
    {
        var expectedSegment = $"{criteriaName}:{criteriaValue}";
        return groupKey.Split('-').Any(segment => segment == expectedSegment);
    }
}