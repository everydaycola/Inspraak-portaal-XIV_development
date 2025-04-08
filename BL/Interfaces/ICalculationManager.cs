using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICalculationManager
{
    public int CalculateSuccessfulRegistrationCount(Guid panelId);
    public Dictionary<string, Dictionary<string, int>> CalculateAllCriteriaCountForPanel(Guid panelId);
    public int CalculatePanelSize(int citizenCount, double samplePercentage);
    public int CalculateAmountOfReserve(int panelSize, double reservePercentage);
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate);
}