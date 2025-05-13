using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto.ProjectPage;

public class TextPostDto
{
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Titel is verplicht")]
    [StringLength(300, ErrorMessage = "Titel mag maximaal 300 karakters bevatten")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Inhoud is verplicht")]
    [StringLength(10000, ErrorMessage = "Inhoud mag maximaal 10000 karakters bevatten")]
    public string Content { get; set; }
    
    public bool VisibleForPanelMember { get; set; }
    
    public bool InformPeopleViaMail { get; set; }
}
