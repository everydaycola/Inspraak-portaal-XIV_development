using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public class MeetingPost : Post, IValidatableObject
{
    public DocumentCollection Documents { get; set; } = new();

    // Forward the validation to the Documents collection
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        return Documents.Validate(validationContext);
    }
}