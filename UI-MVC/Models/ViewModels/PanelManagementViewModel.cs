using Domain.CitizenPanel;
using UI_MVC.Models.Dto;

namespace UI_MVC.Models.ViewModels;

public class PanelManagementViewModel
{
    public Guid PanelId { get; set; }
    public int CitizenCount { get; set; }
    public int PanelSize { get; set; }
    public int AmountOfReserveInvites { get; set; }
    public int TotalInvitesNeeded { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public bool AnyCrossCriteria { get; set; }
    public bool AnyUnknownCriteria { get; set; }
    public ExtraCriteriaDto ExtraCriteriaDto { get; set; } 
}