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
    public int CalculateSuccessfulRegistrationCount(Guid panelId)
    {
        return _panelManager.GetAllPanelMembersForPanel(panelId)
            .Where(m => m.HasRegistered)
            .Distinct()
            .Count();
    }

    // public int CalculateAmountOfMembersInPanel(Guid panelId)
    // {
    //     return _panelManager.GetAllPanelMembersForPanel(panelId).Count();
    // }
    public Dictionary<string, Dictionary<string, int>> CalculateAllCriteriaCountForPanel(Guid panelId)
    {
        var allCriteria = _critManager.GetAllCriteriaWithValuesForPanel(panelId).Criteria;
        var criteriaMemberCount = new Dictionary<string, Dictionary<string, int>>();
        foreach (var crit in allCriteria)
        {
            var answerCounts = new Dictionary<string, int>();
            foreach (var answer in crit.AnswerOptions)
            {
                answerCounts[answer.Option] = CalculateAmountOfMembersWithSpecificCriteria(panelId, answer.Option);
            }
            criteriaMemberCount[crit.Name] = answerCounts;
        }
        return criteriaMemberCount;
    }
    private int CalculateAmountOfMembersWithSpecificCriteria(Guid panelId, string searchedCriteriaValue)
    {
        return _critManager.GetPanelMembersWithCompletedCriteriaGroupedByResponse(panelId)
            .Where(group => group.Key.Contains(searchedCriteriaValue))
            .SelectMany(group => group.Value)
            .Count();
    }
    
    public int CalculatePanelSize(int citizenCount, double samplePercentage)
    {
        //CitizenCount = amount of citizens in gemeente.
        return (int)(citizenCount * samplePercentage);
    }
    public int CalculateAmountOfReserve(int panelSize, double reservePercentage)
    {
        //panelSize = calculatedByCalculatePanelSize
        return (int)(panelSize * reservePercentage);
    }
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate)
    {
        //basePanelSize = calculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of response to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }
}