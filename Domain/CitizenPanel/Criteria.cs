using System.ComponentModel.DataAnnotations;

namespace Domain.CitizenPanel;

public class Criteria : IValidatableObject
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Criteria moet een naam hebben.")]
    [MinLength(2, ErrorMessage = "Criteria naam moet minimaal 2 karakters lang zijn.")]
    [MaxLength(20, ErrorMessage = "Criteria naam mag maximaal 20 karakters lang zijn.")]
    public string Name { get; set; }

    [MinLength(6, ErrorMessage = "Criteria vraag moet minimaal 3 karakters lang zijn.")]
    [MaxLength(100, ErrorMessage = "Criteria vraag mag maximaal 20 karakters lang zijn.")]
    public string Question { get; set; }

    
    public bool IsDefault { get; set; }

    [Required(ErrorMessage = "Criteria moet antwoord opties hebben.")]
    [MinLength(2, ErrorMessage = "Criteria vraag moet minimaal 2 opties hebben.")]
    [MaxLength(12, ErrorMessage = "Criteria vraag mag maximaal 12 opties hebben.")]
    public ICollection<CriteriaAnswerOption> AnswerOptions { get; set; } = new List<CriteriaAnswerOption>();

    // for custom validation (requires IValidateObject interface)
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();

        if (!IsDefault && Question == null)
        {
            validationResults.Add(
                new ValidationResult(
                    "Niet standaard criteria moet een vraag hebben",
                    [nameof(IsDefault), nameof(Question)]));
        }
        return validationResults;
    }
}