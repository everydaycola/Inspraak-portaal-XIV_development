using DAL.EF;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CitizenPanelDbContext _context;
    public UserRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    public ApplicationUser ReadUser(string userId)
    {
        return _context.Users.SingleOrDefault(u => u.Id == userId);
    }

    public IdentityRole ReadUserRole(string userId)
    {
        var roleId =  _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .Single();
        return _context.Roles.Single(r => r.Id == roleId);
    }
}