using DAL;
using DAL.EF;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UI_MVC.Models;

public class ApplicationUserStore(
    CitizenPanelDbContext context,
    Organisation organisation,
    IdentityErrorDescriber? describer = null)
    : UserStore<ApplicationUser>(context,
        describer)
{
    public override Task<IdentityResult> CreateAsync(ApplicationUser user,
        CancellationToken cancellationToken = new CancellationToken())
    {
        //Programma verkiest values die reeds een waarde hebben bv. uit Seeder.
        if (string.IsNullOrEmpty(user.OrganisationId))
        {
            user.OrganisationId = organisation.Id;
        }
        return base.CreateAsync(user,
            cancellationToken);
    }
}