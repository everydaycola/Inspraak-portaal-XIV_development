using Domain.Admin;

namespace DAL.Interfaces;

public interface IConfigurationRepository
{
    //READ
    public GeneralSetting ReadPlatformSettings();
    //CREATE
    //UPDATE
    void UpdatePlatformSettings(GeneralSetting updatedPlatformSettings);
    //DELETE
}