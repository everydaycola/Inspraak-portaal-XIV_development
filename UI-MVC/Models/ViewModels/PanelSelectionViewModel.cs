using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class PanelSelectionViewModel
{
    public IEnumerable<Panel> Panels { get; set; }
    public string ReturnAction { get; set; }
}