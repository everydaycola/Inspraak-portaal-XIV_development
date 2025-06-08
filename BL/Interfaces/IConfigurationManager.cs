using Domain.Admin;

namespace BL.Interfaces;

public interface IConfigurationManager
{
    //GETS
    public GeneralSetting GetPlatformSettings();
    //SAVES
    public void SavePlatformSettings(string discoverConceptText, string aboutInspraakPortaalText);
    //ADD
    //DELETE
}