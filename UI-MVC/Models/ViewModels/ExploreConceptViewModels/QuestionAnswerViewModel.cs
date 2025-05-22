using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionAnswerViewModel
{
    [Key] public int QuestionId { get; set; }
    public int SelectedAnswerOptionId { get; set; }
}