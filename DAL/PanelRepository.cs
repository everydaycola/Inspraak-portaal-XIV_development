using DAL.EF;
using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class PanelRepository : ISubRepository
{
    private readonly CitizenPanelDbContext _context;

    public PanelRepository(CitizenPanelDbContext context)
    {
        this._context = context;
    }

    public Panel readPanel(Guid id)
    {
        return _context.Panels.First(p => p.Id == id);
    }
    public void CreatePanel(Panel panel)
    {
        _context.Panels.Add(panel);
        _context.SaveChanges();
    }

    public void DeletePanel(Panel panel)
    {
        _context.Panels.Remove(panel);
        _context.SaveChanges();
    }

    public Panel ReadPanel(Guid id)
    {
        return _context.Panels.First(p => p.Id == id);
    }
    public Panel ReadPanelWithRepresentationGroup(Guid id)
    {
        return _context.Panels
            .Include(p => p.RepresentationGroup)
            .First(p => p.Id == id);
    }
    
    public void CreatePanelMember(PanelMember panelMember)
    {
        _context.PanelMembers.Add(panelMember);
        _context.SaveChanges();
    }

    public PanelMember ReadPanelMember(Guid id)
    {
        return _context.PanelMembers.First(p => p.Id == id);
    }

    public void DeletePanelMember(PanelMember member)
    {
        _context.PanelMembers.Remove(member);
        _context.SaveChanges();
    }

    public IEnumerable<Panel> ReadAllPanels()
    {
        return _context.Panels.ToList();
    }
}