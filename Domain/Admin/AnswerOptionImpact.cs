using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Question;

namespace Domain.Admin;

public class AnswerOptionImpact
{
    [Key] public int Id { get; set; }
    public AnswerOption AnswerOption { get; set; }
    public ParticipationMethod ParticipationMethod { get; set; }
    public int ImpactWeight { get; set; }
}