using DAL;
using Domain.Tenant;

namespace BL.Interfaces;

public interface IOrganisationManager
{
    public Organisation GetOrganisationById(string id);
    IEnumerable<Organisation> GetAllOrganisations();
    Organisation AddOrganisation(string organisationId, string name, string backgroundColor, string backgroundImage, string logoImageName,bool isTextColorWhite);
    Organisation UpdateOrganisation(string organisationId, string name, string backgroundColor, string backgroundImage, string logoImageName, bool isTextColorWhite);
    void DeleteOrganisation(string organisationId);
}