using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface IPinCRepository
{
    Task<Dictionary<string, string>> GetCommuneNamesAsync();
    Task<Dictionary<string, string>> GetDataFromAPI(string filter);
}