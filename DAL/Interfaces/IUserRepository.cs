using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces;

public interface IUserRepository
{
    public ApplicationUser ReadUser(string userId);
    public IdentityRole ReadUserRole(string userId);
    Panel ReadPanelForUser(string userId);
}