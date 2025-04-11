using Microsoft.AspNetCore.Identity;
using UI_MVC;

namespace Domain;

public class ApplicationUser : IdentityUser, IOrganisational
{
    public string OrganisationId { get; set; }
}