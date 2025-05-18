using System.Security.Claims;
using DAL.EF;
using DAL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

    public Panel ReadPanelForUser(string userId)
    {
        var user = ReadUser(userId);
        if (user != null)
        {
            return _context.PanelMembers
                .Include(pm => pm.Panel)
                .Where(pm => pm.User.Id == user.Id)
                .Select(pm => pm.Panel)
                .SingleOrDefault();
        }

        throw new ArgumentException("User not found for id " + userId);
    }
}