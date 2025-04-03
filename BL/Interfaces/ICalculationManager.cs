using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICalculationManager
{
    public int CalculateSuccesfulRegistrationCount(Guid panelId);
    public int CalculateAmountOfMembersInPanel(Guid panelId);
    public Dictionary<string, Dictionary<string, int>> CalculateAllCriteriaCountForPanel(Guid panelId);
    public int CalculatePanelSize(int citizenCount, double samplePercentage);
    public int CalculateAmountOfReserve(int panelSize, double samplePercentage);
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate);
}