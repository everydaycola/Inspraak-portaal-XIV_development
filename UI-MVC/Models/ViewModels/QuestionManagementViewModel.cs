using Domain.Interfaces;

namespace UI_MVC.Models.ViewModels;

public class QuestionManagementViewModel
{
    public int Id { get; set; }
    public string Question { get; set; }
    public ICollection<AnswerOption> AnswerOptions { get; set; }
}