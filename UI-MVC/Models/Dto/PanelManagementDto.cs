namespace UI_MVC.Models.Dto;

public class PanelManagementDto
{
    public string PanelName { get; set; }
    public int PanelSize { get; set; }
    public int AmountOfAcceptedInvites { get; set; }

    public PanelManagementDto(string panelName, int panelSize, int amountOfAcceptedInvites)
    {
        PanelName = panelName;
        PanelSize = panelSize;
        AmountOfAcceptedInvites = amountOfAcceptedInvites;
    }
}