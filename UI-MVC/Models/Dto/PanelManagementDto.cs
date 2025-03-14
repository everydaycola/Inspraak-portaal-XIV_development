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
    }
    
    
    
}