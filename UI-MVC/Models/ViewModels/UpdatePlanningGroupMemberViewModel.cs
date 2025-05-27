using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class UpdatePlanningGroupMemberViewModel
{
    [Required] public Guid PanelId { get; set; }
    [Required] public Guid UserId { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Naam { get; set; }
    [Required] public string Functie { get; set; }
}