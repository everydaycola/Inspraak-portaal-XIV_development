namespace UI_MVC.Models.Dto;

public class PanelManagementDto
{
    public string PanelName { get; set; }
    public int AmountOfAcceptedInvites { get; set; }
    public int CitizenCount { get; set; }
    public int PanelSize { get; set; }
    public int AmountOfReserveInvites { get; set; }
    public int TotalInvitesNeeded { get; set; }

    public PanelManagementDto(string panelName, int citizenCount, int amountOfAcceptedInvites)
    {
        PanelName = panelName;
        AmountOfAcceptedInvites = amountOfAcceptedInvites;
        this.CitizenCount = citizenCount;
        PanelSize = CalculatePanelSize(citizenCount, 0.005);
        AmountOfReserveInvites = CalculateAmountOfReserve(PanelSize, 0.2);
        TotalInvitesNeeded = CalculateTotalInvitesNeeded(PanelSize + AmountOfReserveInvites, 0.05);
    }
    
    public int CalculatePanelSize(int citizenCount, double samplePercentage)
    {
        //CitizenCount = amount of citizens in gemeente.
        return (int)(citizenCount * samplePercentage);
    }
    
    public int CalculateAmountOfReserve(int panelSize, double samplePercentage)
    {
        //panelSize = calculatedByCalculatePanelSize
        return (int) (panelSize * samplePercentage);
    }
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate)
    {
        //basePanelSize = claculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of resposne to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }
    
    
}