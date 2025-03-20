using Domain.CitizenPanel;

namespace UI_MVC.Models;

public class ExtraCriteriaViewModel
{
    public IEnumerable<CriteriaGroup> CriteriaGroups { get; set; }
    public Dictionary<string, Dictionary<string, int>> CriteriaMemberCount { get; set; }
    public int SuccesfulRegistrationCount { get; set; }
}