using DAL;
using Domain.CitizenPanel;

namespace BL;

public class PanelManager : ISubManager
{
    private readonly PanelRepository _repo;

    public PanelManager(IRepository repo)
    {
        _repo = (PanelRepository) repo;
    }

    public void AddPanel(Panel panel)
    {
        _repo.CreatePanel(panel);
    }

    public Panel GetPanel(Guid id)
    {
        return _repo.ReadPanel(id);
    }

    public Panel GetPanelWithRepresentationGroup(Guid id)
    {
        return _repo.ReadPanelWithRepresentationGroup(id);
    }
    public ICollection<Panel> GetAllPanels()
    {
        return _repo.ReadAllPanels();
    }

    public void RemovePanel(Panel panel)
    {
        _repo.DeletePanel(panel);
    }

    public void AddPanelMember(PanelMember member)
    {
        if (_repo.ReadPanel(member.Panel.Id)!=null)
        {
            _repo.CreatePanelMember(member);   
        }
        else
        {
            throw new Exception("Panel with id " + member.Panel.Id +" does not exist");
        }
    }

    public PanelMember GetPanelMember(Guid id)
    {
        return _repo.ReadPanelMember(id);
    }
    
    public void RemovePanelMember(PanelMember member)
    {
        _repo.DeletePanelMember(member);
    }
}