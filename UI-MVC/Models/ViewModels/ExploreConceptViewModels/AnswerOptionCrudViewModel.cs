using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class AnswerOptionCrudViewModel
{
    [Key] public int Id { get; set; }
    public string AnswerOptionText { get; set; }
    public int Weight { get; set; }
}