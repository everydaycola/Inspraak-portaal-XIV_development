using Domain.CitizenPanel;
using UI_MVC.Models.Dto;

namespace UI_MVC.Models;

public class ExtraCriteriaViewModel
{
    public uniqueCodesDto uniqueCodesDto { get; set; }
    public Dictionary<string, Dictionary<string, int>> CriteriaMemberCount { get; set; }
    public int SuccesfulRegistrationCount { get; set; }
    public double TotalMemberCount { get; set; }
}