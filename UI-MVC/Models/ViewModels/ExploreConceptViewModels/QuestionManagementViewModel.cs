using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionManagementViewModel
{
    [Key] public int Id { get; set; }
    public string Question { get; set; }
    public List<AnswerOptionCrudViewModel> AnswerOptions { get; set; }
}