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

    public Organisation AddOrganisation(string organisationId, string name, string backgroundColor,
        string backgroundImage)
    {
        Organisation newOrganisation = new Organisation()
        {
            Id = organisationId,
            Name = name,
            BackgroundColor = backgroundColor,
            BackgroundImage = backgroundImage
        };
        return _repo.CreateOrganisation(newOrganisation);
    }

    public Organisation UpdateOrganisation(string organisationId, string name, string backgroundColor,
        string backgroundImage)
    {
        var existingOrganisation = _repo.ReadOrganisationById(organisationId);
        if (existingOrganisation != null)
        {
            existingOrganisation.Name = name;
            existingOrganisation.BackgroundColor = backgroundColor;
            existingOrganisation.BackgroundImage = backgroundImage;
            return _repo.UpdateOrganisation(existingOrganisation);
        }

        throw new InvalidOperationException($"$No organisation found with ID: {organisationId}");
    }

    public void DeleteOrganisation(string organisationId)
    {
        _repo.RemoveOrganisation(organisationId);
    }
}