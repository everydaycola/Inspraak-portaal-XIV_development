using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class UniqueCodesDto
{
    public Guid memberId { get; set; }
    public ICollection<Criteria> criteria{ get; set; }
    public string GroupKey => string.Join(", ", criteria.Select(crit => crit.Value));

    public UniqueCodesDto(Guid memberId)
    {
        this.memberId = memberId;
        this.criteria = new List<Criteria>();
    }
}