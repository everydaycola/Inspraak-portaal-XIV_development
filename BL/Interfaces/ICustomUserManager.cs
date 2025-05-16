using System.Security.Claims;
using Domain;
using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICustomUserManager
{
    public Panel getPanelForUser(string userId);
    public ApplicationUser ReadUser(string userId);
}