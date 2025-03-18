using DAL.EF;
using DAL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CriteriaRepository :ICriteriaRepository
{
    private readonly CitizenPanelDbContext _context;

    public CriteriaRepository(CitizenPanelDbContext context)
    {
        this._context = context;
    }

    public IEnumerable<Criteria> ReadAllCriteriaWithValuesForPanel(Guid panelId)
    {
        return _context.Criteria
            .Include(c => c.Values)
            .Where(c => c.Panel.Id == panelId)
            .ToList();
    }
    public IEnumerable<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId)
    {
        return this._context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
            .ThenInclude(pm => pm.Panel)
            .Where(cg => cg.PanelMembers.Any(pm => pm.Panel.Id == panelId))
            .ToList();
    }

    public CriteriaGroup ReadCriteriaGroupForPanel(Guid panelId, string groupName)
    {
        return this._context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
            .ThenInclude(pm => pm.Panel)
            .FirstOrDefault(cg => cg.PanelMembers.Any(p => p.Panel.Id == panelId) && cg.Name == groupName);
    }

    public CriteriaGroup ReadCriteraGroupByPanelIdAndName(Guid panelId, string groupName)
    {
        return _context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
            .ThenInclude(pm => pm.Panel)
            .SingleOrDefault(cg => 
                cg.Name == groupName &&
                cg.PanelMembers.Any(pm => pm.Panel.Id == panelId)
            );
    }

    public IEnumerable<Criteria> ReadAllDefaultCriteriaWithValuesForPanel(Guid panelId)
    {
        return _context.Criteria
            .Include(c => c.Values)
            .Where(c => c.Panel.Id == panelId && c.IsDefault == false)
            .ToList();
    }

    public void UpdateCriteriaGroup(CriteriaGroup criteriaGroup)
    {
        _context.CriteriaGroups.Update(criteriaGroup);
        _context.SaveChanges();
    }

    public void CreateCriteriaGroup(CriteriaGroup newCriteriaGroup)
    {
        _context.CriteriaGroups.Add(newCriteriaGroup);
        _context.SaveChanges();
    }
}