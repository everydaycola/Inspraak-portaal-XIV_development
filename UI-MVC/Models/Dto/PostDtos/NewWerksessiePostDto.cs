using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto.PostDtos;

public class NewWerksessiePostDto : IValidatableObject
{
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Titel is verplicht")]
    [StringLength(300, ErrorMessage = "Titel mag maximaal 300 karakters bevatten")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Datum is verplicht")]
    public DateTime SessionDate { get; set; }
    
    [Required(ErrorMessage = "Tijdstip is verplicht")]
    [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Ongeldig tijdstip.")]
    public string SessionTime { get; set; }
    
    public bool InformPeopleViaMail { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();
        
        if (SessionDate < DateTime.Now.Date)
        {
            validationResults.Add(
                new ValidationResult(
                    "De datum mag niet in het verleden liggen.",
                    [nameof(SessionDate)]));
        }
        
        return validationResults;
    }
}