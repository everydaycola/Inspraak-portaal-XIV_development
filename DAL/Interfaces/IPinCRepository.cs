using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface IPinCRepository
{
    public Task<Dictionary<string, string>> GetCommuneNamesAsync();
    public Task<Dictionary<string, string>> GetDataFromApi(string filter);
}