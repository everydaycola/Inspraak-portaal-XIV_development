using Domain.CitizenPanel;

namespace UI_MVC.Models.Dto;

public class uniqueCodesDto
{
    public Guid panelId { get; set; }
    public Dictionary<string, ICollection<PanelMember>> panelMembers { get; set; }
}