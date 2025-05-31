using Domain;
using Microsoft.AspNetCore.Identity;
using UI_MVC.Models;

namespace UI_MVC;

public class IdentitySeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentitySeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedRoles();
        
        var admin = new ApplicationUser
        {
            Email = "admin@ip14.com",
            UserName = "admin@ip14.com"
        };
        await _userManager.CreateAsync(admin, "Admin123!");
        await _userManager.AddToRoleAsync(admin, CustomIdentityConstants.AdminRole);

        var organisatie1 = new ApplicationUser
        {
            Email = "panels@antwerpen.be",
            UserName = "panels@antwerpen.be",
            OrganisationId = "antwerpen"
        };
        await _userManager.CreateAsync(organisatie1, "Antwerpen123!");
        await _userManager.AddToRoleAsync(organisatie1, CustomIdentityConstants.OrganisatieRole);

    }

    private async Task SeedRoles()
    {
        var roles = new[] { CustomIdentityConstants.AdminRole,CustomIdentityConstants.OrganisatieRole, CustomIdentityConstants.PanelMemberRole};
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}