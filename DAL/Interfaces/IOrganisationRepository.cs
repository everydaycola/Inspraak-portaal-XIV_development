namespace DAL.Interfaces;

public interface IOrganisationRepository
{
    public Organisation ReadOrganisationById(string id);
    public IEnumerable<Organisation> GetAllOrganisations();
    public Organisation UpdateOrganisation(Organisation existingOrganisation);
}