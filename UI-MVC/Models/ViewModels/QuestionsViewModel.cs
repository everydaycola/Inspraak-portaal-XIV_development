using Domain.Interfaces;

namespace UI_MVC.Models.ViewModels;

public class QuestionsViewModel
{
    public int Id { get; set; }
    public string Question { get; set; }
    public int Weight { get; set; }
    public List<AnswerOptionCrudViewModel> AnswerOptions { get; set; }
}