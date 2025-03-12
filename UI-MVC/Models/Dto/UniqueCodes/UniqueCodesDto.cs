using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class UniqueCodesDto
{
    public Guid memberId { get; set; }
    public Guid panelId { get; set; }
    public ICollection<Criteria> criteria{ get; set; }
    public string generatedUri => "http://localhost:5228/Register?UserId=" + memberId + "&PanelId=" + panelId;
    public string GroupKey => string.Join(", ", criteria.Select(crit => crit.Name +"-"+ crit.Value));

    public UniqueCodesDto(Guid panelId, Guid memberId)
    {
        this.panelId = panelId;
        this.memberId = memberId;
        this.criteria = new List<Criteria>();
    }
}