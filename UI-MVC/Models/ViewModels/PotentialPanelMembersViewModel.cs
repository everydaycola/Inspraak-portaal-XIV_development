using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class PotentialPanelMembersViewModel
{
    public Guid PanelId { get; set; }
    public List<PanelMember> PanelMembers { get; set; }
}