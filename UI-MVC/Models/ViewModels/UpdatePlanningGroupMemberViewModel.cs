using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class UpdatePlanningGroupMemberViewModel
{
    [Required(ErrorMessage = "User ID is verplicht")] 
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Email is verplicht")] 
    [EmailAddress(ErrorMessage = "Email is niet geldig")] 
    public string Email { get; set; }
    [Required] 
    [MaxLength(300, ErrorMessage = "Naam mag maximaal 300 karakters bevatten")]
    public string Naam { get; set; }
    [Required] 
    [MaxLength(300, ErrorMessage = "Functie mag maximaal 300 karakters bevatten")]
    public string Functie { get; set; }
}