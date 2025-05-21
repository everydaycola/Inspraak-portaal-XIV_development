using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class AnswerOptionCrudViewModel
{
    [Key] public int Id { get; set; }
    public string AnswerOptionText { get; set; }
    public double Weight { get; set; }
}