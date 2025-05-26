using System.Linq.Expressions;
using DAL.EF;
using DAL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CriteriaRepository : ICriteriaRepository
{
    private readonly CitizenPanelDbContext _context;

    public CriteriaRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }
    
    // READ

    // counts how many members have answers what how many times. 
    // outer key is criteria name, inner key is answer name, int is count
    // !!WARNING!! does not give any values for criteria that have 0 responses.
    public Dictionary<string, Dictionary<string, int>> ReadAllCriteriaMemberCountsWithValuesForPanel(Guid panelId, bool onlyUnknown = false)
    {
        return _context.PanelMembers
            .Where(panelMember => panelMember.Panel.Id == panelId)
            .Where(panelMember => panelMember.HasRegistered)
            .SelectMany(panelMember => panelMember.Responses)
            .GroupBy(response => response.Criteria.Name)
            .ToDictionary(
                criteriaGroup => criteriaGroup.Key,
                criteriaGroup => criteriaGroup
                    .Where(cr => !onlyUnknown || !cr.Criteria.IsDistributionKnown)
                    .GroupBy(r => r.SelectedOption)
                    .ToDictionary(
                        optionGroup => optionGroup.Key,
                        optionGroup => optionGroup.Count()
                    )
            );
    }

    public IEnumerable<Criteria> ReadAllCriteriaForPanelWithAnswerOptions(Guid panelId, bool onlyDefault = false, bool includeKnown = true, bool includeUnknown = true)
    {
        
        return _context.Panels
            .Where(p => p.Id == panelId)
            .SelectMany(p => p.Criteria)
            .Include(c => c.AnswerOptions)
            .Where(c => !onlyDefault || c.IsDefault)
            .Where(c => includeUnknown || c.IsDistributionKnown)
            .Where(c => includeKnown || !c.IsDistributionKnown)
            .ToList();
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