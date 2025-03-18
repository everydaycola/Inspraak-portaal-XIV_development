using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class UniqueCodesDto
{
    public Guid MemberId { get; set; }
    public Guid PanelId { get; set; }
    public ICollection<Criteria> Criteria{ get; set; }
    public string GeneratedUri => "/Register?UserId=" + MemberId;
    public CriteriaGroup CriteriaGroup { get; set; }

    public UniqueCodesDto(Guid panelId, Guid memberId, CriteriaGroup criteriaGroup)
    {
        PanelId = panelId;
        MemberId = memberId;
        Criteria = new List<Criteria>();
        CriteriaGroup = criteriaGroup;
    }
}