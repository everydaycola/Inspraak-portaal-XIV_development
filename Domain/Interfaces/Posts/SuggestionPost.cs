using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Posts.PostItems;

namespace Domain.Interfaces.Posts;

public class SuggestionPost : Post, IValidatableObject
{
    public ICollection<Suggestion> Suggestions { get; set; } = [];
    [Required(ErrorMessage = "Post moet een lijst van documenten hebben")]
    public DocumentCollection Documents { get; set; } = new();
    
    // Forward the validation to the Documents collection
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        return Documents.Validate(validationContext);
    }
}