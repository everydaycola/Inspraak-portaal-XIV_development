using Domain.CitizenPanel;
using UI_MVC.Models.Dto;

namespace UI_MVC.Models.ViewModels;

public class PeopleManagementViewModel
{
    public Guid PanelId { get; set; }
    public UniqueCodesViewModel UniqueCodesViewModel { get; set; }
    public IEnumerable<PlanningGroupMember> PlanningGroupMembers { get; set; }
    public ExtraCriteriaViewModel ExtraCriteriaViewModel { get; set; } 
    public PotentialPanelMembersViewModel PotentialPanelMembersViewModel { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public int PanelSize { get; set; }
    public bool AnyCrossCriteria { get; set; }
    public bool AnyUnknownCriteria { get; set; }
    public int AmountOfReserveInvites { get; set; }
    public int TotalInvitesNeeded { get; set; }
}