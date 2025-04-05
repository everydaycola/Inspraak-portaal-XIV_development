using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class PanelManagementDto
{
    public Guid PanelId { get; set; }
    public string PanelName { get; set; }
    public int CitizenCount { get; set; }
    public int PanelSize { get; set; }
    public int SuccesfulRegistrationCount { get; set; }
    public int AmountOfReserveInvites { get; set; }
    public int TotalInvitesNeeded { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public ExtraCriteriaViewModel ExtraCriteriaViewModel { get; set; }
    public IEnumerable<PlanningGroupMember> PlanningGroupMembers { get; set; }

    public PanelManagementDto(Guid panelId, string panelName, int citizenCount, int amountOfAcceptedInvites)
    {
        PanelId = panelId;
        PanelName = panelName;
        SuccesfulRegistrationCount = amountOfAcceptedInvites;
        CitizenCount = citizenCount;
        ExtraCriteriaViewModel = new ExtraCriteriaViewModel();
    }
    
    
    
}