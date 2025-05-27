namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels.client;

public class ExploreConceptViewModel
{
    public List<QuestionsViewModel> Questions { get; set; } = new List<QuestionsViewModel>();
    public List<QuestionAnswerViewModel> SubmittedAnswers { get; set; } = new List<QuestionAnswerViewModel>();
}