using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class PlanningGroupMemberViewModel
{
    
    [Required(ErrorMessage = "Panel ID is verplicht")]
    public Guid PanelId { get; set; }
    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Naam is verplicht")]
    public string Naam { get; set; }
    [Required(ErrorMessage = "Functie is verplicht")]
    public string Functie { get; set; }
}