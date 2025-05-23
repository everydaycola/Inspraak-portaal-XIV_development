using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto.PostDtos;

public class NewDocumentPostViewDto : IValidatableObject
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".pdf", ".txt", ".dockx" };
    
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Titel is verplicht")]
    [StringLength(300, ErrorMessage = "Titel mag maximaal 300 karakters bevatten")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Bestand is verplicht")]
    public IFormFile File { get; set; }
    
    public bool VisibleForPanelMember { get; set; }
    
    public bool InformPeopleViaMail { get; set; }
    
    public bool IsGloballyVisible { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();
        // get extension
        var extension = Path.GetExtension(File.FileName).ToLowerInvariant();
        // check if extension allowed
        if (AllowedExtensions.Contains(extension)) return validationResults;
        validationResults.Add(
            new ValidationResult(
                "Ongeldig bestandstype. Toegestane types: afbeeldingen, pdf, txt, dockx. Uw bestands type: " + extension,
                [nameof(File)]));
        return validationResults;
    }
}