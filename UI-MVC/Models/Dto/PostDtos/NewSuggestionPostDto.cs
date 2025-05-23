using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels.PostViewModels;

public class NewSuggestionPostDto
{
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Tijdlijn ID is required")]
    public Guid TimeLineId { get; set; }
    
    [Required(ErrorMessage = "Titel is verplicht")]
    [StringLength(300, ErrorMessage = "Titel mag maximaal 300 karakters bevatten")]
    public string Title { get; set; }
    
    public bool VisibleForPanelMember { get; set; }
    
    public bool InformPeopleViaMail { get; set; }
}