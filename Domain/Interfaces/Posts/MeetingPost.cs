using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public class MeetingPost : Post, IValidatableObject
{
    [Required(ErrorMessage = "Meeting post moet een lijst van documenten hebben")]
    public ICollection<string> DocumentNames { get; set; } = [];
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        if (DocumentNames == null) return [];
        var validationResults = new List<ValidationResult>();
        foreach (var documentName in DocumentNames)
        {
            if (documentName != null && documentName.Length > 300)
            {
                validationResults.Add( new ValidationResult("Document naam is te lang", [nameof(DocumentNames)]));
            }
        }
        return validationResults;
    }
}