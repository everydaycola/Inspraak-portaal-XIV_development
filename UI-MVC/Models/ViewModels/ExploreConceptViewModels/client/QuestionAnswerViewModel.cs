using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels.client;

public class QuestionAnswerViewModel
{
    [Key] public int QuestionId { get; set; }
    public int SelectedAnswerOptionId { get; set; }
}