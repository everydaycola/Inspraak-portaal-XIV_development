using BL.Interfaces;

namespace BL.Managers;

public class CalculationManager : ICalculationManager
{
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