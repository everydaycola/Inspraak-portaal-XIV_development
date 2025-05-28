namespace Domain.Interfaces.Question;

public class QuestionWeightTips
{
    public int Id { get; set; }
    public int MinScore { get; set; }
    public int MaxScore { get; set; }
    public string Message { get; set; }
}