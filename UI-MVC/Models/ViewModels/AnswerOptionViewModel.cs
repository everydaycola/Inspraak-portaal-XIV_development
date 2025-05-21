namespace UI_MVC.Models.ViewModels;

public class AnswerOptionViewModel
{
    public int Id { get; set; }
    public string AnswerOptionText { get; set; }
    public double Weight { get; set; }
    public int QuestionManagementViewModelId { get; set; }
    public QuestionManagementViewModel QuestionManagementViewModel { get; set; }
}