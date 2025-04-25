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
        var percentageMenData = await _pinCRepository.GetPercentageOfMenAsync();
        var higherEducationData = await _pinCRepository.GetHigherEducationAsync();

        return populationData
            .Where(p => gemeenteNamen.ContainsKey(p.ExternalCode))
            .Select(p => new Commune
            {
                CommuneCode = p.ExternalCode,
                CommuneName = gemeenteNamen[p.ExternalCode],
                TotalPopulation = p.ValueString,
                PercentageMen = percentageMenData.GetValueOrDefault(p.ExternalCode),
                HigherEducation = higherEducationData.GetValueOrDefault(p.ExternalCode),
            })
            .ToList();
    }
}