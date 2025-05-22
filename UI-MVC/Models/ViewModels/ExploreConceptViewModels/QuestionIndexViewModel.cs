namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionIndexViewModel
{
    public List<QuestionManagementViewModel> Questions { get; set; } = new List<QuestionManagementViewModel>();
    public QuestionManagementViewModel QuestionToEdit { get; set; } = new QuestionManagementViewModel();

    public List<QuestionWeightTipsViewModel> QuestionWeightTips { get; set; } = new List<QuestionWeightTipsViewModel>();
    public QuestionWeightTipsViewModel QuestionWeightTipViewModelToEdit { get; set; } = new QuestionWeightTipsViewModel();
}