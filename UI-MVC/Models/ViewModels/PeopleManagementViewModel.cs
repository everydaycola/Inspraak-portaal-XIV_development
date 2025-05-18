using Domain.CitizenPanel;
using UI_MVC.Models.Dto;

namespace UI_MVC.Models.ViewModels;

public class PeopleManagementViewModel
{
    public Guid PanelId { get; set; }
    public uniqueCodesDto UniqueCodesDto { get; set; }
    public IEnumerable<PlanningGroupMember> PlanningGroupMembers { get; set; }
    public ExtraCriteriaDto ExtraCriteriaDto { get; set; } 
    public bool IsRegistrationOpen { get; set; }
    public int PanelSize { get; set; }
    public bool AnyCrossCriteria { get; set; }
    public bool AnyUnknownCriteria { get; set; }
    public int AmountOfReserveInvites { get; set; }
    public int TotalInvitesNeeded { get; set; }
}