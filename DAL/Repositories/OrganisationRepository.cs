using DAL.EF;
using DAL.Interfaces;

namespace DAL.Repositories;

public class OrganisationRepository : IOrganisationRepository
{
    private readonly CitizenPanelDbContext _context;

    public OrganisationRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    public Organisation ReadOrganisationById(string id)
    {
        return _context.Organisations.SingleOrDefault(o => o.Id == id);
    }

    public IEnumerable<Organisation> ReadAllOrganisations()
    {
        return _context.Organisations.ToList();
    }

    public Organisation UpdateOrganisation(Organisation existingOrganisation)
    {
        _context.Organisations.Update(existingOrganisation);
        _context.SaveChanges();
        return ReadOrganisationById(existingOrganisation.Id);
    }

    public void RemoveOrganisation(string organisationId)
    {
        var organisation = ReadOrganisationById(organisationId);
        if (organisation != null)
        {
            _context.Organisations.Remove(organisation);
            _context.SaveChanges();
        }
    }
}