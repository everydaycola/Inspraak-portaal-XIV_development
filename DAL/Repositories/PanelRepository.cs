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

    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Responses)
            .Single(p => p.PanelMemberId == id);
    }
    
    public PanelMember ReadPanelMemberWithPanel(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .Include(pm => pm.Responses)
                .ThenInclude(pm => pm.Criteria)
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
        return _context.PanelMembers
            .Include(pm => pm.Panel)
                .ThenInclude(p => p.Criteria)
                .ThenInclude(c => c.AnswerOptions)
            .Include(pm => pm.Responses)
            .Where(pm => pm.Panel.Id == id) 
            .ToList();
    }

    public IEnumerable<PanelMember> ReadPanelMembersWhichAnsweredAllQuestionsWithCriteria(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .ThenInclude(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .Include(pm => pm.Responses)
            .Where(pm => pm.Panel.Id == id && pm.HasAnsweredAllQuestions == true) 
            .ToList();
    }
    
    public IEnumerable<Panel> ReadAllPanels()
    {
        return _context.Panels.ToList();
    }
    
    public PanelMember UpdatePanelMember(PanelMember member)
    {
        _context.PanelMembers.Update(member);
        _context.SaveChanges();

        return ReadPanelMemberWithPanel(member.PanelMemberId);
    }
    
    public void CreatePanelMember(PanelMember panelMember)
    {
        _context.PanelMembers.Add(panelMember);
        _context.SaveChanges();
    }

    public ICollection<PanelMember> ReadPanelMembersAndRepresentationGroup(Guid id)
    {
        return _context.PanelMembers.Include(pm => pm.Panel)
            .ThenInclude(pm => pm.RepresentationGroup)
            .Where(pm => pm.Panel.Id == id)
            .ToList();
    }
    
    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.Criteria)
            .ThenInclude(p => p.AnswerOptions)
            .Single(p => p.Id == panelId);
    }

    public void UpdatePanel(Panel panel)
    {
        _context.Panels.Update(panel);
        _context.SaveChanges();
    }

}