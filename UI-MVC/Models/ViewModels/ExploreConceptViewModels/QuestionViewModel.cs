using System.Collections;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionViewModel
{
    public string QuestionText { get; set; }
    public ICollection<AnswerOptionViewModel> AnswerOptions { get; set; }
    public IEnumerable<ParticipationViewModel> ParticipationMethods { get; set; }
}