using Microsoft.AspNetCore.Identity;

namespace Domain;

public class ApplicationUser : IdentityUser, IOrganisational
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string organisationId)
    {
        OrganisationId = organisationId;
    }

    public string OrganisationId { get; set; }
}