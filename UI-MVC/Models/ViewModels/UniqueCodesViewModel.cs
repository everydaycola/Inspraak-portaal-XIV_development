using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class UniqueCodesViewModel
{
    public Guid panelId { get; set; }
    public Dictionary<string, Dictionary<int, List<PanelMember>>> panelMembers { get; set; }
    public int Phases { get; set; } = 1;
}