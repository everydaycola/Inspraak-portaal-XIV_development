using DAL;
using Domain.CitizenPanel;

namespace BL;

public class PanelManager : ISubManager
{
    private readonly PanelRepository _repo;

    public PanelManager(IRepository repo)
    {
        _repo = (PanelRepository)repo;
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

    public IEnumerable<Panel> GetAllPanels()
    {
        return _repo.ReadAllPanels();
    }

    public void UpdatePanel(Guid id, bool isRegistrationOpen)
    {
        var panel = _repo.readPanel(id);
        if (panel != null)
        {
            panel.IsRegistrationOpen = isRegistrationOpen;
            _repo.UpdatePanel(panel);
            return;
        }

        throw new NullReferenceException("Panel with id: " + id + " was not found.");
    }

    public void RemovePanel(Panel panel)
    {
        _repo.DeletePanel(panel);
    }

    public void AddPanelMember(PanelMember member)
    {
        if (_repo.ReadPanel(member.Panel.Id) != null)
        {
            _repo.CreatePanelMember(member);
        }
        else
        {
            throw new Exception("Panel with id " + member.Panel.Id + " does not exist");
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

    public int CalculatePanelSize(int citizenCount, double samplePercentage)
    {
        //CitizenCount = amount of citizens in gemeente.
        return (int)(citizenCount * samplePercentage);
    }

    public int CalculateAmountOfReserve(int panelSize, double samplePercentage)
    {
        //panelSize = calculatedByCalculatePanelSize
        return (int)(panelSize * samplePercentage);
    }

    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate)
    {
        //basePanelSize = claculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of resposne to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }
}