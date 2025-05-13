using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UI_MVC.Models.Dto.ProjectPage;

public class SummaryDto
{
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Meeting ID is required")]
    public Guid MeetingId { get; set; }
    
    [Required(ErrorMessage = "Verslag bestand is verplicht")]
    public IFormFile VerslagFile { get; set; }
}
