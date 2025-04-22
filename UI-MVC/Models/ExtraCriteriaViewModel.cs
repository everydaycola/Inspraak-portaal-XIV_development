namespace UI_MVC.Models;

public class ExtraCriteriaViewModel
{
    public Dictionary<string, Dictionary<string, int>> CriteriaMemberCount { get; set; }
    public Dictionary<string, Dictionary<string, double>> DesiredCriteriaCount { get; set; }
    public int SuccessfulRegistrationCount { get; set; }
    public int DesiredRegistrationCount { get; set; }
}