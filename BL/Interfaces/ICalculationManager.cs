using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICalculationManager
{
    public double CalculateTotalMemberCount(Guid panelId);
    public Dictionary<string, double> CalculateAllCriteriaCountForPanel(Guid panelId);
    public double CalculateAmountOfMembersWithSpecificCriteria(Guid panelId, string searchedCriteriaName,
        string searchedCriteriaValue);
}