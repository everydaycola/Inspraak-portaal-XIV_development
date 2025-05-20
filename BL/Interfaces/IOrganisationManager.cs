using DAL;

namespace BL.Interfaces;

public interface IOrganisationManager
{
    public Organisation GetOrganisationById(string id);
    IEnumerable<Organisation> GetAllOrganisations();
    Organisation UpdateOrganisation(string organisationId, string name, string backgroundColor, string backgroundImage, string logoImageName);
    void DeleteOrganisation(string organisationId);
}