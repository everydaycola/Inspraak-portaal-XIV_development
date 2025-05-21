using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public class Question : IQuestion
{
    [Key] public int Id { get; set; }
    [Required] public string QuestionText { get; set; }
    [Required] public ICollection<AnswerOption> AnswerOptions { get; set; }
}