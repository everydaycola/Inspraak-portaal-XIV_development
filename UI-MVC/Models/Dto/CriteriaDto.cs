using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto;

public class CriteriaDto
{
    [Required(ErrorMessage = "Criteria moet een naam hebben.")]
    [MinLength(2, ErrorMessage = "Criteria naam moet minimaal 2 karakters lang zijn.")]
    [MaxLength(20, ErrorMessage = "Criteria naam mag maximaal 20 karakters lang zijn.")]
    public string Name { get; set; }
    [MinLength(6, ErrorMessage = "Criteria vraag moet minimaal 3 karakters lang zijn.")]
    [MaxLength(100, ErrorMessage = "Criteria vraag mag maximaal 20 karakters lang zijn.")]
    public string Question { get; set; }
    public bool IsDefault { get; set; }
    public bool IsDistributionKnown { get; set; }

    [Required(ErrorMessage = "Criteria moet antwoord opties hebben.")]
    [MinLength(2, ErrorMessage = "Criteria vraag moet minimaal 2 opties hebben.")]
    [MaxLength(12, ErrorMessage = "Criteria vraag mag maximaal 12 opties hebben.")]
    public ICollection<CriteriaAnswerOptionDto> AnswerOptions { get; set; } = new List<CriteriaAnswerOptionDto>();
}