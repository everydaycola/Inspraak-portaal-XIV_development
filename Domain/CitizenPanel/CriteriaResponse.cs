using System.ComponentModel.DataAnnotations;

namespace Domain.CitizenPanel;

public class CriteriaResponse
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Antwoord moet aan een vraag gelinkt zijn.")]
    public Criteria Criteria { get; set; }

    [Required(ErrorMessage = "Je moet een antwoord selecteren.")]
    [MinLength(2, ErrorMessage = "Antwoord moet minimaal 2 karakters lang zijn.")]
    [MaxLength(20, ErrorMessage = "Antwoord mag maximaal 20 karakters lang zijn.")]
    public string SelectedOption { get; set; }
}