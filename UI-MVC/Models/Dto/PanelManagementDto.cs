namespace UI_MVC.Models.Dto;

public class PanelManagementDto
{
    public Guid PanelId { get; set; }
    public string PanelName { get; set; }
    public int AmountOfAcceptedInvites { get; set; }
    public int CitizenCount { get; set; }
    public int PanelSize { get; set; }
    public int AmountOfReserveInvites { get; set; }
    public int TotalInvitesNeeded { get; set; }

    public PanelManagementDto(Guid panelId, string panelName, int citizenCount, int amountOfAcceptedInvites)
    {
        this.PanelId = panelId;
        PanelName = panelName;
        AmountOfAcceptedInvites = amountOfAcceptedInvites;
        this.CitizenCount = citizenCount;
    }
    
    
    
}