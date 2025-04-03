using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces;

public interface IUserRepository
{
    public IdentityUser ReadUser(string userId);
    public IdentityRole ReadUserRole(string userId);
}