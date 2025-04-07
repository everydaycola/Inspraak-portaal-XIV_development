namespace DAL.Interfaces;

public interface IOrganisationRepository
{
    public Organisation ReadOrganisationById(string id);
}