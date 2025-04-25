using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICustomUserManager
{
    public Panel getPanelForUser(string userId);
}