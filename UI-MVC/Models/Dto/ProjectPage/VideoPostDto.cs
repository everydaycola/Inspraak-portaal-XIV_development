using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UI_MVC.Models.Dto.ProjectPage;

public class VideoPostDto: IValidatableObject
{
    private static readonly Regex Regex = new Regex(
        @"(?:youtube(?:-nocookie)?\.com/(?:[^/]+/.+/|(?:v|e(?:mbed)?)/|.*[?&]v=)|youtu\.be/)([^""&?/\s]{11})", 
    RegexOptions.IgnoreCase);
    
    [Required(ErrorMessage = "Panel ID is required")]
    public Guid PanelId { get; set; }
    
    [Required(ErrorMessage = "Titel is verplicht")]
    [StringLength(300, ErrorMessage = "Titel mag maximaal 300 karakters bevatten")]
    public string Title { get; set; }
    
    public string YoutubeUrl { get; set; }
    
    public string VideoUrl { get; set; }
    
    public bool VisibleForPanelMember { get; set; }
    
    public bool InformPeopleViaMail { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();
        
        if (string.IsNullOrWhiteSpace(YoutubeUrl) && string.IsNullOrWhiteSpace(VideoUrl))
        {
            validationResults.Add(
                new ValidationResult(
                    "Geef een url in.",
                    [nameof(YoutubeUrl), nameof(VideoUrl)]));
        } else if (!string.IsNullOrWhiteSpace(YoutubeUrl) && !string.IsNullOrWhiteSpace(VideoUrl))
        {
            validationResults.Add(
                new ValidationResult(
                    "U heeft zowel een youtube als video url ingegeven.",
                    [nameof(YoutubeUrl), nameof(VideoUrl)]));
        } else if (!string.IsNullOrWhiteSpace(VideoUrl) && (!Uri.TryCreate(VideoUrl, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)))
            {
                validationResults.Add(
                    new ValidationResult(
                        "Uw url is geen publieke url, de url moet met http of https beginnen. Uw url begint met " + uriResult.Scheme,
                        [nameof(VideoUrl)]));
        }
        
        return validationResults;
    }
}
