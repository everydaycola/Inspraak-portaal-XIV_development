using Domain.CitizenPanel;

namespace UI_MVC.Models;

public class ExtraCriteriaViewModel
{
    public IEnumerable<CriteriaGroup> CriteriaGroups { get; set; }
    public Dictionary<string, double> CriteriaMemberCount { get; set; }
    public double TotalMemberCount { get; set; }
}