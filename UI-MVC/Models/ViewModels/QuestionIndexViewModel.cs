namespace UI_MVC.Models.ViewModels;

public class QuestionIndexViewModel
{
    public List<QuestionManagementViewModel> Questions { get; set; } = new List<QuestionManagementViewModel>();
    public QuestionManagementViewModel QuestionToEdit { get; set; } = new QuestionManagementViewModel();
}