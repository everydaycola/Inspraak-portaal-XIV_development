using System.ComponentModel.DataAnnotations;
using Domain.Admin;

namespace Domain.Interfaces.Question;

public class AnswerOption
{
    [Key] public int Id { get; set; }
    [Required] public string AnswerOptionText { get; set; }
    [Required] public int Weight { get; set; }
    public List<AnswerOptionImpact> Impacts { get; set; }
    public Question Question { get; set; }
    
}