using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class UniqueCodesDto
{
    public Guid memberId { get; set; }
    public Guid panelId { get; set; }
    public ICollection<Criteria> criteria{ get; set; }
    public string generatedUri => "http://localhost:5228/Register?UserId=" + memberId;
    public CriteriaGroup CriteriaGroup { get; set; }

    public UniqueCodesDto(Guid panelId, Guid memberId, CriteriaGroup criteriaGroup)
    {
        this.panelId = panelId;
        this.memberId = memberId;
        this.criteria = new List<Criteria>();
        this.CriteriaGroup = criteriaGroup;
    }
}