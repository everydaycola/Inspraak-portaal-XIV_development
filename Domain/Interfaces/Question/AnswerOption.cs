using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public class AnswerOption
{
    [Key] public int Id { get; set; }
    [Required] public string AnswerOptionText { get; set; }
    [Required] [Range(0, int.MaxValue)] public double Weight { get; set; }
    public int QuestionId { get; set; }
    public Question Question { get; set; }
}