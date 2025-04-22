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

    // public Panel ReadAllCriteriaWithValuesForPanel(Guid panelId)
    // {
    //     return _context.Panels.Include(p => p.Criteria)
    //         .ThenInclude(c => c.AnswerOptions)
    //         .Single(p => p.Id == panelId);
    // }

    // gives a list of criteria for a panel with the options as a list
    public Dictionary<string, IEnumerable<string>> ReadAllCriteriaNamesAndOptions(Guid panelId)
    {
        return _context.Panels
            .Where(p => p.Id == panelId)
            .Include(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .SelectMany(p => p.Criteria)
            .GroupBy(c => c.Name)
            .ToDictionary(
                group => group.Key,
                group => group
                    .SelectMany(c => c.AnswerOptions
                        .Select(ao => ao.Option))
            );
    }

    // counts how many members have answers what how many times. 
    // outer key is criteria name, inner key is answer name, int is count
    // !!WARNING!! does not give any values for criteria that have 0 responses.
    public Dictionary<string, Dictionary<string, int>> ReadAllCriteriaMemberCountsWithValuesForPanel(Guid panelId)
    {
        return _context.PanelMembers
            .Where(panelMember => panelMember.Panel.Id == panelId)
            .Where(panelMember => panelMember.HasRegistered)
            .SelectMany(panelMember => panelMember.Responses)
            .GroupBy(response => response.Criteria.Name)
            .ToDictionary(
                criteriaGroup => criteriaGroup.Key,
                criteriaGroup => criteriaGroup
                    .GroupBy(r => r.SelectedOption)
                    .ToDictionary(
                        optionGroup => optionGroup.Key,
                        optionGroup => optionGroup.Count()
                    )
            );
    }
    
    public Dictionary<string, Dictionary<string, double>> ReadAllDesiredCriteriaPercentages(Guid panelId)
    {
        return _context.Panels
            .Where(p => p.Id == panelId)
            .Include(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .SelectMany(p => p.Criteria)
            .GroupBy(c => c.Name)
            .ToDictionary(
                group => group.Key,
                group => group
                    .SelectMany(c => c.AnswerOptions)
                    .ToDictionary(
                        o => o.Option,
                        o => o.DistributionPercentage)
            );
    }


    // public IEnumerable<Criteria> ReadAllNonDefaultCriteriaWithValuesForPanel(Guid panelId)
    // {
    //     return _context.Panels
    //         .Where(p => p.Id == panelId)
    //         .SelectMany(p => p.Criteria)
    //         .Include(c => c.AnswerOptions)
    //         .ToList();
    //     
    // }

    // public Criteria ReadCriteriaByName(Guid panelId, string critName)
    // {
    //     return _context.Panels
    //         .Where(p => p.Id == panelId)
    //         .SelectMany(p => p.Criteria)
    //         .FirstOrDefault(c => c.Name == critName);
    // }

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