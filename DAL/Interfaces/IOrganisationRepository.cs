namespace DAL.Interfaces;

public interface IOrganisationRepository
{
    public Organisation ReadOrganisationById(string id);
    Organisation CreateOrganisation(Organisation newOrganisation);
    IEnumerable<Organisation> ReadAllOrganisations();
    Organisation UpdateOrganisation(Organisation existingOrganisation);
    void RemoveOrganisation(string organisationId);
}