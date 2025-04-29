using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICommuneManager
{
    Task<List<Commune>> GetCommunes();
}