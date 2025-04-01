using Microsoft.AspNetCore.Identity;

namespace UI_MVC;

public class IdentitySeeder
{
    private readonly UserManager<IdentityUser> _userManager;

    public IdentitySeeder(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        var admin = new IdentityUser("admin@test.com")
        {
            Email = "admin@test.com"
        };
        await _userManager.CreateAsync(admin, "Admin123!");
    }
}