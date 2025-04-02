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

    public int CalculateAmountOfMembersInPanel(Guid panelId)
    {
        return _panelManager.GetAllPanelMembersForPanel(panelId).Count();
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
        var responseGroups = _critManager.GetPanelMembersWithCompletedCriteriaGroupedByResponse(panelId);
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
    
    public int CalculatePanelSize(int citizenCount, double samplePercentage)
    {
        //CitizenCount = amount of citizens in gemeente.
        return (int)(citizenCount * samplePercentage);
    }
    public int CalculateAmountOfReserve(int panelSize, double samplePercentage)
    {
        //panelSize = calculatedByCalculatePanelSize
        return (int)(panelSize * samplePercentage);
    }
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate)
    {
        //basePanelSize = calculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of response to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }
}