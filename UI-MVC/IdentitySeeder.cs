using Microsoft.AspNetCore.Identity;

namespace UI_MVC;

public class IdentitySeeder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentitySeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedRoles();
        
        var admin = new IdentityUser("admin@test.com")
        {
            Email = "admin@test.com"
        };
        await _userManager.CreateAsync(admin, "Admin123!");
        await _userManager.AddToRoleAsync(admin, "Admin");

        var organisatie1 = new IdentityUser("user@antwerpen.be")
        {
            Email = "user@antwerpen.be"
        };
        await _userManager.CreateAsync(organisatie1, "Antwerpen123!");
        await _userManager.AddToRoleAsync(organisatie1, "Organisatie");

    }

    private async Task SeedRoles()
    {
        var roles = new[] { "Admin","Organisatie"};
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}