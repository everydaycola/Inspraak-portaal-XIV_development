using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICalculationManager
{
    public int CalculateSuccesfulRegistrationCount(Guid panelId);
    public Dictionary<string, Dictionary<string, int>> CalculateAllCriteriaCountForPanel(Guid panelId);
}