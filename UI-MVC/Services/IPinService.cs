using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Services;

public interface IPinCService
{
    Task<List<PopulationRecord>> GetPopulationDataAsync();
    Task<Dictionary<string, string>> GetGemeenteNamenAsync();
}
