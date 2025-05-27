using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Posts.PostItems;

namespace Domain.Interfaces.Posts;

public class SuggestionPost : Post, IValidatableObject
{
    public ICollection<Suggestion> Suggestions { get; set; } = [];
    public bool IsVotingOpen { get; set; }
    public double VotingMajorityFactor { get; set; }
    
    [Required(ErrorMessage = "Suggestion post moet een lijst van documenten hebben")]
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