using DAL.EF;
using DAL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PanelRepository : IPanelRepository
{
    private readonly CitizenPanelDbContext _context;

    public PanelRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    public Panel ReadPanel(Guid id)
    {
        return _context.Panels.Find(id);
    }

    public PanelMember ReadPanelMember(Guid id)
    {
        return _context.PanelMembers.Find(id);
    }
    
    public PanelMember ReadPanelMemberWithPanel(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .Single(p => p.PanelMemberId == id);
    }

    public Panel ReadPanelWithRepresentationGroup(Guid id)
    {
        return _context.Panels
            .Include(p => p.RepresentationGroup)
            .Single(p => p.Id == id);
    }

    public IEnumerable<PanelMember> ReadPanelMembersWithCriteria(Guid id)
    {
        return _context.PanelMembers.Include(pm => pm.Panel)
            .ThenInclude(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .Where(pm => pm.Panel.Id == id)
            .ToList();
    }

    public IEnumerable<Panel> ReadAllPanels()
    {
        return _context.Panels.ToList();
    }

    public PanelMember ReadPanelByUserId(Guid memberId)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .Single(pm => pm.PanelMemberId == memberId);
    }

    public PanelMember UpdatePanelMember(PanelMember member)
    {
        _context.PanelMembers.Update(member);
        _context.SaveChanges();

        return ReadPanelMemberWithPanel(member.PanelMemberId);
    }

    public void CreatePanel(Panel panel)
    {
        _context.Panels.Add(panel);
        _context.SaveChanges();
    }
    
    public void CreatePanelMember(PanelMember panelMember)
    {
        _context.PanelMembers.Add(panelMember);
        _context.SaveChanges();
    }

    public void DeletePanel(Panel panel)
    {
        _context.Panels.Remove(panel);
        _context.SaveChanges();
    }

    public void DeletePanelMember(PanelMember member)
    {
        _context.PanelMembers.Remove(member);
        _context.SaveChanges();
    }

    public PanelMember ReadPanelWithMembersAndRepresentationGroup(Guid id)
    {
        return _context.PanelMembers.Include(pm => pm.Panel)
            .ThenInclude(pm => pm.RepresentationGroup)
            .Single(pm => pm.Panel.Id == id);
    }

    public void CreateCriteria(Criteria criteria)
    {
        _context.Criteria.Update(criteria);
        _context.SaveChanges();
    }

    public void UpdatePanel(Panel panel)
    {
        _context.Panels.Update(panel);
        _context.SaveChanges();
    }

}