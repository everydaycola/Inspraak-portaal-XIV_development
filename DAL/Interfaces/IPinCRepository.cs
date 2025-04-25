using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface IPinCRepository
{
    Task<List<PopulationRecord>> GetPopulationDataAsync();
    Task<Dictionary<string, string>> GetCommuneNamesAsync();
    Task<Dictionary<string, string>> GetPercentageOfMenAsync();
    Task<Dictionary<string, string>> GetHigherEducationAsync();
}