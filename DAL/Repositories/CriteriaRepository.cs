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
        _context = context;
    }

    public IEnumerable<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId)
    {
        return _context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
            .ThenInclude(pm => pm.Panel)
            .Where(cg => cg.PanelMembers.Any(pm => pm.Panel.Id == panelId))
            .ToList();
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
    
}