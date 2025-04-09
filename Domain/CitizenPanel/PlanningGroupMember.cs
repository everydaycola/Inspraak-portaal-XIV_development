using Microsoft.AspNetCore.Identity;

namespace Domain.CitizenPanel;

public class PlanningGroupMember
{
    public Guid Id { get; set; }
    public Panel Panel { get; set; }
    public ApplicationUser User { get; set; }
}