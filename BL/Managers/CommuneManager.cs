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

    // ALLE filters zijn te vinden op provincies.incijfers.be > zoek wat je wilt > info en dan onder code
    public async Task<List<Commune>> GetCommunes()
    {
        var gemeenteNamen = await _pinCRepository.GetCommuneNamesAsync();
        var populationData = await _pinCRepository.GetDataFromAPI("v1111a_tot_bevolking");
        var percentageMenData = await _pinCRepository.GetDataFromAPI("vp1111a_mannen");
        var middleSchoolStudents = await _pinCRepository.GetDataFromAPI("v2302_so_lln");
        var higherEducationData = await _pinCRepository.GetDataFromAPI("v2390_hoog");
        var workingData = await _pinCRepository.GetDataFromAPI("v1201_ksz_werkend");
        var lookingForWorkData = await _pinCRepository.GetDataFromAPI("v1201_ksz_werkzoekend");
        var notWorkingData = await _pinCRepository.GetDataFromAPI("v1201_ksz_nba");
        var caughtDrinkingAndDrivingData = await _pinCRepository.GetDataFromAPI("v2803_alco");
        var peopleInjuredInTrafficAccident = await _pinCRepository.GetDataFromAPI("v2802_t");
        var deathsByTrafficAccident = await _pinCRepository.GetDataFromAPI("v2802_vh_ver_do");
        var totalRegisteredCats = await _pinCRepository.GetDataFromAPI("v3702_kat");
        return populationData
            .Where(p => gemeenteNamen.ContainsKey(p.Key))
            .Select(p => new Commune
            {
                CommuneCode = p.Key,
                CommuneName = gemeenteNamen[p.Key],
                TotalPopulation = p.Value,
                PercentageMen = percentageMenData.GetValueOrDefault(p.Key),
                SecondarySchoolSctudents = middleSchoolStudents.GetValueOrDefault(p.Key),
                HigherEducation = higherEducationData.GetValueOrDefault(p.Key),
                TotalPeopleWorking = workingData.GetValueOrDefault(p.Key),
                TotalPeopleLookingForWork = lookingForWorkData.GetValueOrDefault(p.Key),
                TotalNotWorking = notWorkingData.GetValueOrDefault(p.Key),
                CaughtDrivingWhileDrunk = caughtDrinkingAndDrivingData.GetValueOrDefault(p.Key),
                PeopleInjuredInTrafficAccident = peopleInjuredInTrafficAccident.GetValueOrDefault(p.Key),
                DeathsByTrafficAccident = deathsByTrafficAccident.GetValueOrDefault(p.Key),
                TotaLRegisteredCats = totalRegisteredCats.GetValueOrDefault(p.Key),
            })
            .ToList();
    }
}