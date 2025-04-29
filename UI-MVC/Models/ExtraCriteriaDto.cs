using Domain.CitizenPanel;

namespace UI_MVC.Models;

public class ExtraCriteriaDto
{
    public Dictionary<string, int> CriteriaGroupAbsoluteMemberCount { get; set; }
    public Dictionary<string, Dictionary<string, int>> CriteriaMemberCount { get; set; }
    public Dictionary<string, Dictionary<string, double>> DesiredCriteriaCount { get; set; }
    public ICollection<Criteria> Criteria { get; set; }
    public int SuccessfulRegistrationCount { get; set; }
    public int DesiredRegistrationCount { get; set; }
}