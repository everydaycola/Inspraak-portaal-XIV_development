using DAL;

namespace BL.Interfaces;

public interface IOrganisationManager
{
    public Organisation GetOrganisationById(string id);
}