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
    /*public IEnumerable<CriteriaGroup> ReadAllCriteriaGroupForPanel(Guid panelId)
    {
        return _context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
                .ThenInclude(pm => pm.Panel)
            .Include(cg=>cg.CriteriaAnswers)
                .ThenInclude(ca => ca.CriteriaValue)
            .Include(c => c.CriteriaAnswers)
                .ThenInclude(ca=>ca.Criteria)
            .Where(cg => cg.PanelMembers.Any(pm => pm.Panel.Id == panelId))
            .ToList();
    }*/
    
    /*public CriteriaGroup ReadCriteriaGroupByMemberId(Guid memberId)
    {
        return _context.CriteriaGroups
            .Include(cg => cg.CriteriaAnswers)
            .ThenInclude(ca => ca.CriteriaValue)
            .ThenInclude(ca => ca.Criteria)    
            .FirstOrDefault(cg => cg.PanelMembers.Any(pm => pm.PanelMemberId == memberId));
    }*/

    /*public CriteriaGroup ReadCriteriaGroupForPanel(Guid panelId, string groupName)
    {
        return this._context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
            .ThenInclude(pm => pm.Panel)
            .FirstOrDefault(cg => cg.PanelMembers.Any(p => p.Panel.Id == panelId) && cg.Name == groupName);
    }*/

    /*public CriteriaGroup ReadCriteriaGroupByid(Guid criteriaGroupId)
    {
        return _context.CriteriaGroups
            .Include(cg => cg.PanelMembers)
            .Single(cg => cg.Id == criteriaGroupId);
    }*/

    public IEnumerable<Criteria> ReadAllNonDefaultCriteriaWithValuesForPanel(Guid panelId)
    {
        return _context.Panels
            .Where(p => p.Id == panelId)
            .SelectMany(p => p.Criteria)
            .Include(c => c.AnswerOptions)
            .ToList();
        
    }

    /*public void UpdateCriteriaGroup(CriteriaGroup criteriaGroup)
    {
        _context.CriteriaGroups.Update(criteriaGroup);
        _context.SaveChanges();
    }*/

    /*public void CreateCriteriaGroup(CriteriaGroup newCriteriaGroup)
    {
        _context.CriteriaGroups.Add(newCriteriaGroup);
        _context.SaveChanges();
    }*/

    /*public CriteriaValue ReadCriteriaValueBasedOnCriteriaAndValue(Guid criteriaId, string criteriaValue)
    {
        return _context.CriteriaValues
            .FirstOrDefault(cv => cv.Criteria.CriteriaId == criteriaId && cv.Value == criteriaValue);
    }*/

    public Criteria ReadCriteriaByName(Guid panelId, string critName)
    {
        return _context.Panels
            .Where(p => p.Id == panelId)
            .SelectMany(p => p.Criteria)
            .FirstOrDefault(c => c.Name == critName);
    }
    
}