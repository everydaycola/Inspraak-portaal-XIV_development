using DAL.EF;
using DAL.Interfaces;
using Domain.Admin;

namespace DAL.Repositories;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly CitizenPanelDbContext _context;

    public ConfigurationRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }
    //READ
    public GeneralSetting ReadPlatformSettings()
    {
        return _context.GeneralSettings.FirstOrDefault();
    }
    //CREATE
    //UPDATE
    //DELETE
}