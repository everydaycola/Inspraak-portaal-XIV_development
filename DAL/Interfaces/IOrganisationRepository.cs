namespace DAL.Interfaces;

public interface IOrganisationRepository
{
    public Organisation ReadOrganisationById(string id);
    IEnumerable<Organisation> GetAllOrganisations();
    Organisation UpdateOrganisation(Organisation existingOrganisation);
}