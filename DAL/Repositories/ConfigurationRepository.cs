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
    public void UpdatePlatformSettings(GeneralSetting updatedPlatformSettings)
    {
        var existingSetting = _context.GeneralSettings.FirstOrDefault();
        if (existingSetting == null)
        {
            _context.GeneralSettings.Add(updatedPlatformSettings);
        }
        else
        {
            existingSetting.AboutInspraakPortaalText = updatedPlatformSettings.AboutInspraakPortaalText;
            existingSetting.DiscoverConceptText = updatedPlatformSettings.DiscoverConceptText;
            
        }
        _context.SaveChanges();
    }
    //DELETE
}