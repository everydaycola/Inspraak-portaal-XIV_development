using System.ComponentModel.DataAnnotations;

namespace Domain.CitizenPanel;

public class CriteriaAnswerOption
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Antwoord optie moet een naam hebben.")]
    [MinLength(4, ErrorMessage = "Antwoord optie moet minimaal 4 karakters lang zijn.")]
    [MaxLength(20, ErrorMessage = "Antwoord optie mag maximaal 20 karakters lang zijn.")]
    public string Option { get; set; }

    [Required(ErrorMessage = "Antwoord optie moet een verdeling waarde hebben.")]
    [Range(0, 1, ErrorMessage = "Sample rate moet een percentage tussen 0 en 100% zijn.")]
    public double DistributionPercentage { get; set; }
}