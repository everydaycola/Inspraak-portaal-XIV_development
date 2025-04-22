using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class PlanningGroupMemberViewModel
{
    [Required]
    public Guid PanelId { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Naam { get; set; }
    [Required]
    public string Functie { get; set; }
}