using DAL;
using Domain.CitizenPanel;

namespace BL;

public class PanelManager : ISubManager
{
    private readonly PanelRepository _repo;

    public PanelManager(PanelRepository repo)
    {
        _repo = repo;
    }

    public void addPanel(Panel panel)
    {
        _repo.createPanel(panel);
    }

    public Panel getPanel(Guid id)
    {
        return _repo.readPanel(id);
    }
}