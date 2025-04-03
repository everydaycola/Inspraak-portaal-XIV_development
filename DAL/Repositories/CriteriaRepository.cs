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

    public Panel ReadAllCriteriaWithValuesForPanel(Guid panelId)
    {
        return _context.Panels.Include(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .Single(p => p.Id == panelId);
    }
    public IEnumerable<Criteria> ReadAllNonDefaultCriteriaWithValuesForPanel(Guid panelId)
    {
        return _context.Panels
            .Where(p => p.Id == panelId)
            .SelectMany(p => p.Criteria)
            .Include(c => c.AnswerOptions)
            .ToList();
        
    }
    
    public Criteria ReadCriteriaByName(Guid panelId, string critName)
    {
        return _context.Panels
            .Where(p => p.Id == panelId)
            .SelectMany(p => p.Criteria)
            .FirstOrDefault(c => c.Name == critName);
    }

    public Criteria ReadCriteriaByNameWithAnswerOptions(Guid panelId, string critName)
    {
        return _context.Panels
            .Include(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .Where(p => p.Id == panelId)
            .SelectMany(p => p.Criteria)
            .FirstOrDefault(c => c.Name == critName);
    }
    
}