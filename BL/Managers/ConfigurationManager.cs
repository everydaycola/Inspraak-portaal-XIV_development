using BL.Interfaces;
using DAL.Interfaces;
using Domain.Admin;

namespace BL.Managers;

public class ConfigurationManager : IConfigurationManager
{
    private readonly IConfigurationRepository _configurationRepository;

    public ConfigurationManager(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public GeneralSetting GetPlatformSettings()
    {
        return _configurationRepository.ReadPlatformSettings();
    }
}