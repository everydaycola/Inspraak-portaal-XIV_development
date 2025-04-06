using Microsoft.AspNetCore.Identity;
using UI_MVC;

namespace Domain.CitizenPanel;

public class Panel : IOrganisational
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Criteria> Criteria { get; set; }
    public RepresentationGroup RepresentationGroup { get; set; }
    public double SampleRate { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public int SuccesfulRegistrationCount { get; set; }
    public ApplicationUser Owner { get; set; }
    public string OrganisationId { get; set; }
}