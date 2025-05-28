using System.ComponentModel.DataAnnotations;
using Domain.GlobalDtos;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels.management;

public class QuestionViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Vraagtekst moet ingevuld zijn.")]
    [Length(0,255, ErrorMessage="Vraagtekst mag maximaal 255 karakters lang zijn.")]
    public string QuestionText { get; set; }
    [MinLength(2, ErrorMessage = "Een vraag moet minstens 2 antwoord opties bevatten.")]
    public List<AnswerOptionDto> AnswerOptions { get; set; }
    public IEnumerable<ParticipationViewModel> ParticipationMethods { get; set; }
}