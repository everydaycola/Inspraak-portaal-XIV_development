using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class PotentialPanelMembersViewModel
{
    public Guid panelId { get; set; }
    public Dictionary<string, Dictionary<int, List<PanelMember>>> panelMembers { get; set; }
}