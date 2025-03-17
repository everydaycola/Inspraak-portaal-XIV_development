namespace UI_MVC.Models.Dto.Register;

public class ExtraQuestionFormAnswersDto
{
    public Guid UserId { get; set; }
    public Guid PanelId { get; set; }
    public string Email { get; set; }
    public Dictionary<string, string> CriteriaAnswers { get; set; }

    public ExtraQuestionFormAnswersDto()
    {
        this.CriteriaAnswers = new Dictionary<string, string>();
    }
}