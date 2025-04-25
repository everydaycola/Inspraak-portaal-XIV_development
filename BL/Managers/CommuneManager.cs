using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class CommuneManager : ICommuneManager
{
    private readonly IPinCRepository _pinCRepository;

    public CommuneManager(IPinCRepository pinCRepository)
    {
        _pinCRepository = pinCRepository;
    }

    public async Task<List<Commune>> GetCommunes()
    {
        var populationData = await _pinCRepository.GetPopulationDataAsync();
        var gemeenteNamen = await _pinCRepository.GetCommuneNamesAsync();

        return populationData
            .Where(p => gemeenteNamen.ContainsKey(p.ExternalCode))
            .Select(p => new Commune
            {
                CommuneCode = p.ExternalCode,
                CommuneName = gemeenteNamen[p.ExternalCode],
                TotalPopulation = p.ValueString
            })
            .ToList();
    }
}