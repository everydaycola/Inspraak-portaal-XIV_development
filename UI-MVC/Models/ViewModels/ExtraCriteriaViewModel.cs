using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class ExtraCriteriaViewModel
{
    // contains the exact amount of registrations per criteria group
    // only used in cross partial
    public Dictionary<string, int> CriteriaGroupAbsoluteMemberCount { get; set; }
    // contains the exact about of registration per answer per criteria
    // only used in standard parital
    public Dictionary<string, Dictionary<string, int>> CriteriaMemberCount { get; set; }
    public ICollection<Criteria> Criteria { get; set; }
    public int SuccessfulRegistrationCount { get; set; }
    public int DesiredRegistrationCount { get; set; }
}