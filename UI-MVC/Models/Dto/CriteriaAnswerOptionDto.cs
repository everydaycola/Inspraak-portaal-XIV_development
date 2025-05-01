using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto;

public class CriteriaAnswerOptionDto
{
    [Required(ErrorMessage = "Antwoord optie moet een naam hebben.")]
    [MinLength(2, ErrorMessage = "Antwoord optie moet minimaal 2 karakters lang zijn.")]
    [MaxLength(20, ErrorMessage = "Antwoord optie mag maximaal 20 karakters lang zijn.")]
    public string Option { get; set; }
    [Required(ErrorMessage = "Antwoord optie moet een verdeling waarde hebben.")]
    [Range(0, 1, ErrorMessage = "Percentage moet tussen 0 en 100% zijn.")]
    public string DistributionPercentage { get; set; }
}