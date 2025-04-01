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
        var criteriaMemberCount = new Dictionary<string, Dictionary<string, int>>();
        foreach (var crit in allCriteria)
        {
            var answerCounts = new Dictionary<string, int>();
            foreach (var answer in crit.AnswerOptions)
            {
                var count = CalculateAmountOfMembersWithSpecificCriteria(panelId, crit.Name, answer.Option);
                answerCounts[answer.Option] = count;
            }
            criteriaMemberCount[crit.Name] = answerCounts;
        }
        return criteriaMemberCount;
    }
    private int CalculateAmountOfMembersWithSpecificCriteria(Guid panelId, string searchedCriteriaName, string searchedCriteriaValue)
    {
        var responseGroups = _critManager.GetPanelMembersGroupedByResponses(panelId);
        var count =  responseGroups
            .Where(group => GroupContainsCriteria(group.Key, searchedCriteriaName, searchedCriteriaValue))
            .SelectMany(group => group.Value)
            .Count();

        return count;
    }

    private bool GroupContainsCriteria(string groupKey, string criteriaName, string criteriaValue)
    {
        return groupKey.Split('-').Contains(criteriaValue);
    }
}