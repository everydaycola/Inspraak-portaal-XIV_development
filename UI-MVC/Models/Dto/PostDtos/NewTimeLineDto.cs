using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto.PostDtos;

public class NewTimeLineDto
{
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Groep titel is verplicht")]
    [StringLength(300, ErrorMessage = "Groep titel mag maximaal 300 karakters bevatten")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Datum is verplicht")]
    public DateTime SessionDate { get; set; }
}