namespace BL.Interfaces;

public interface ICalculationManager
{
    public int CalculatePanelSize(int citizenCount, double samplePercentage);
    public int CalculateAmountOfReserve(int panelSize, double reservePercentage);
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate);
}