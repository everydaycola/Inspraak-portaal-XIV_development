using Domain.GlobalDtos;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels.management;

public class QuestionViewModel
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public List<AnswerOptionDto> AnswerOptions { get; set; }
    public IEnumerable<ParticipationViewModel> ParticipationMethods { get; set; }
}