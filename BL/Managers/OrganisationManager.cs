using BL.Interfaces;
using DAL;
using DAL.EF;
using DAL.Interfaces;

namespace BL.Managers;

public class OrganisationManager : IOrganisationManager
{
    private readonly IOrganisationRepository _repo;

    public OrganisationManager(IOrganisationRepository repo)
    {
        _repo = repo;
    }

    public Organisation GetOrganisationById(string id)
    {
        return _repo.ReadOrganisationById(id);
    }

    public IEnumerable<Organisation> GetAllOrganisations()
    {
        return _repo.ReadAllOrganisations();
    }

    public Organisation UpdateOrganisation(string organisationId, string name, string backgroundColor, string backgroundImage, string logoImageName)
    {
        var existingOrganisation = _repo.ReadOrganisationById(organisationId);
        if (existingOrganisation != null)
        {
            if (existingOrganisation == null)
                throw new InvalidOperationException($"$No organisation found with ID: {organisationId}");
            existingOrganisation.Name = name;
            if(backgroundImage != ""){
                existingOrganisation.BackgroundImage = backgroundImage;
            }
            if(logoImageName != ""){
                existingOrganisation.LogoImageName = logoImageName;
            }
            _repo.UpdateOrganisation(existingOrganisation);
            return existingOrganisation;
        }

        throw new InvalidOperationException($"$No organisation found with ID: {organisationId}");
    }

    public void DeleteOrganisation(string organisationId)
    {
        var organisation = _repo.ReadOrganisationById(organisationId);
        if (organisation == null) return;
        _repo.RemoveOrganisation(organisation);
    }
}