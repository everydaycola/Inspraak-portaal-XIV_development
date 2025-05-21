using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace UI_MVC.Models.ViewModels;

public class QuestionManagementViewModel
{
    [Key] public int Id { get; set; }
    public string Question { get; set; }
    public List<AnswerOptionCrudViewModel> AnswerOptions { get; set; }
}