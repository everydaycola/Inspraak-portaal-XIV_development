using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public class Question : IQuestion
{
    [Key] public int Id { get; set; }
    [Required] public string QuestionText { get; set; }
    [Required] public int Weight { get; set; }
    [Required] public List<AnswerOption> AnswerOptions { get; set; }
}