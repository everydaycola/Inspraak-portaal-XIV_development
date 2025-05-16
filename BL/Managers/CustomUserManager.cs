using System.Security.Claims;
using BL.Interfaces;
using DAL.Interfaces;
using Domain;
using Domain.CitizenPanel;

namespace BL.Managers;

public class CustomUserManager : ICustomUserManager
{
    public readonly IUserRepository _userRepository;
    public CustomUserManager(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    public Panel getPanelForUser(string userId)
    {
       return _userRepository.ReadPanelForUser(userId);
    }
}