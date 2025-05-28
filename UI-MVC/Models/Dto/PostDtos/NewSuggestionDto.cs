using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto.PostDtos;

public class NewSuggestionDto
{
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Post ID is required")]
    public Guid PostId { get; set; }
    
    [Required(ErrorMessage = "Aanbeveling is required")]
    [StringLength(300, ErrorMessage = "Aanbeveling mag maximaal 300 karakters bevatten")]
    public string Suggestion { get; set; }
}