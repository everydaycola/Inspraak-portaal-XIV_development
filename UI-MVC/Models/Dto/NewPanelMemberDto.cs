using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class NewPanelMemberDto
{
    public string UserId{ get; set; }
    public string PanelId { get; set; }
    public string Email { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public bool HasAnsweredQuestions { get; set; }
    public IEnumerable<Criteria> NonDefaultCriteria { get; set; }
    public IEnumerable<CriteriaAnswer> DefaultCriteria { get; set; }
}