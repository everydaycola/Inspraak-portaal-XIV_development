namespace DAL.Interfaces;

public interface IOrganisationRepository
{
    // READ
    public Organisation ReadOrganisationById(string organisationId);
    public IEnumerable<Organisation> ReadAllOrganisations();
    
    // CREATE
    Organisation CreateOrganisation(Organisation newOrganisation);
    
    // UPDATE
    public void UpdateOrganisation(Organisation existingOrganisation);
    
    // DELETE
    public void RemoveOrganisation(Organisation organisation);
}