using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class NewPanelMemberDto
{
    public string UserId{ get; set; }
    public string PanelId { get; set; }

    public IEnumerable<Criteria> criteriaList { get; set; }
}