using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class uniqueCodesDto
{
    public Guid panelId { get; set; }
    public Dictionary<int, Dictionary<string, List<PanelMember>>> panelMembers { get; set; }
    public int Phases { get; set; } = 1;
}