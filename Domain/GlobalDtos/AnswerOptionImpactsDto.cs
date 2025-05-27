namespace Domain.GlobalDtos;

public class AnswerOptionImpactsDto
{
    public Guid ParticipationMethodId { get; set; }
    public string ParticipationMethodName { get; set; }
    public int ImpactWeight { get; set; }
}