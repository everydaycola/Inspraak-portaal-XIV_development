using DAL.EF;
using Domain.CitizenPanel;

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
    public void createPanel(Panel panel)
    {
        _context.Panels.Add(panel);
        _context.SaveChanges();
    }
    
    
}