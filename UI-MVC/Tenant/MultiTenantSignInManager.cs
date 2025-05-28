using Domain;
using Domain.Tenant;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace UI_MVC.Tenant;

public class MultiTenantSignInManager(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor,
    IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor,
    ILogger<SignInManager<ApplicationUser>> logger,
    IAuthenticationSchemeProvider schemes,
    IUserConfirmation<ApplicationUser> confirmation,
    Organisation organisation
)
    : SignInManager<ApplicationUser>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes,
        confirmation)
{
    public override Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent,
        bool lockoutOnFailure)
    {
        
        if (user.OrganisationId != organisation.Id)
        {
            return Task.FromResult(SignInResult.Failed);
        }

        return base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }
    
};