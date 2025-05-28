using DAL.EF;
using DAL.Interfaces;
using Domain.Tenant;

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

    public Organisation CreateOrganisation(Organisation newOrganisation)
    {
        _context.Organisations.Add(newOrganisation);
        _context.SaveChanges();
        var organisation = _context.Organisations.SingleOrDefault(o => o.Id == newOrganisation.Id);
        return organisation;
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