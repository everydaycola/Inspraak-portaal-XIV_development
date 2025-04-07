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
}