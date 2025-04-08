using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using UI_MVC;

namespace Domain.CitizenPanel;

public class Panel : IOrganisational,IValidatableObject
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Panel moet een naam hebben.")]
    [MinLength(4, ErrorMessage = "Panel naam moet minimaal 4 karakters lang zijn.")]
    [MaxLength(100, ErrorMessage = "Panel naam mag maximaal 100 karakters lang zijn.")]
    public string Name { get; set; }

    [MaxLength(10, ErrorMessage = "Panel mag maximaal 10 criteria hebben.")]
    public ICollection<Criteria> Criteria { get; set; }

    [Required(ErrorMessage = "Panel moet een representatie groep hebben.")]
    public RepresentationGroup RepresentationGroup { get; set; }

    [Required(ErrorMessage = "Panel moet een  hebben sample rate hebben.")]
    [Range(0, 1, ErrorMessage = "Sample rate moet een percentage tussen 0 en 100% zijn.")]
    public double SampleRate { get; set; }

    public bool IsRegistrationOpen { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Succesvol geregistreerde personen mag niet negatief zijn.")]
    public int SuccessfulRegistrationCount { get; set; }
    [Required(ErrorMessage = "Panel moet een eigenaar hebben.")]
    public ApplicationUser Owner { get; set; }
    public string OrganisationId { get; set; }
    
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        return (from c in Criteria
            where Math.Abs(c.AnswerOptions.Select(o => o.DistributionPercentage).Sum() - 1) > 0.001
            select new ValidationResult("De verdeling van de antwoord opties moet 100% zijn. " + 
                                        "Nu: " + c.AnswerOptions.Select(c => c.DistributionPercentage).Sum() * 100,
                [nameof(c)])).ToList();
    }
    
}