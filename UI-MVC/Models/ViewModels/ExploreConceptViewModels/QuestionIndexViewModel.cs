namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionIndexViewModel
{
    public List<QuestionsViewModel> Questions { get; set; } = new List<QuestionsViewModel>();
    public QuestionsViewModel QuestionToEdit { get; set; } = new QuestionsViewModel();

    public List<QuestionWeightTipsViewModel> QuestionWeightTips { get; set; } = new List<QuestionWeightTipsViewModel>();
    public QuestionWeightTipsViewModel QuestionWeightTipViewModelToEdit { get; set; } = new QuestionWeightTipsViewModel();
}