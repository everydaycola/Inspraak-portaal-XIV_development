using DAL;

namespace BL.Interfaces;

public interface IOrganisationManager
{
    public Organisation GetOrganisationById(string id);
    public IEnumerable<Organisation> GetAllOrganisations();
    public Organisation UpdateOrganisation(string organisationId, string name, string backgroundColor, string backgroundImage);
}