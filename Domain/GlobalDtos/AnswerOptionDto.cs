namespace Domain.GlobalDtos;

public class AnswerOptionDto
{
    public int Id { get; set; }
    public string AnswerText { get; set; }
    public List<AnswerOptionImpactsDto> Impacts { get; set; }
}