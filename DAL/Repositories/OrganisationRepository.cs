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

    public Organisation ReadOrganisationById(string organisationId)
    {
        return _context.Organisations.Find(organisationId);
    }

    public IEnumerable<Organisation> ReadAllOrganisations()
    {
        return _context.Organisations.ToList();
    }

    public void UpdateOrganisation(Organisation existingOrganisation)
    {
        _context.Organisations.Update(existingOrganisation);
        _context.SaveChanges();
    }

    public void RemoveOrganisation(Organisation organisation)
    {
        _context.Organisations.Remove(organisation);
        _context.SaveChanges();
    }
}