using Domain.CitizenPanel;
namespace UI_MVC.Models.Dto;

public class UniqueCodesDto
{
    public CriteriaGroup CriteriaGroup { get; set; }

    public UniqueCodesDto( CriteriaGroup criteriaGroup)
    {
        CriteriaGroup = criteriaGroup;
    }
}