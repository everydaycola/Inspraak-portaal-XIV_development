using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class HomePanelsViewModel
{
    public IEnumerable<Panel> Panels { get; set; }
    public string DiscoverConceptText { get; set; }
    public string AboutInspraakPortaalText { get; set; }
}