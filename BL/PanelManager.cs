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

    public Panel GetPanel(Guid id)
    {
        return _repo.ReadPanel(id);
    }

    public PanelMember GetPanelMember(Guid id)
    {
        return _repo.ReadPanelMember(id);
    }

    public void AddPanel(Panel panel)
    {
        _repo.CreatePanel(panel);
    }
    
    public void AddPanel(string name, int size, Dictionary<string, Dictionary<string, double>> distribution)
    {
        var panel = new Panel(name);
        var members = new List<PanelMember>();
        for (int i = 0; i < size; i++)
        {
            PanelMember newMember = new PanelMember();
            newMember.Panel = panel;
            newMember.Criteria = new List<Criteria>();
            members.Add(newMember);
        }
        // for each criteria in the distribution
        foreach (var key in distribution.Keys)
        {
            var categoryEnumerator = distribution[key].Keys.GetEnumerator();
            var memberEnumerator = members.GetEnumerator();

            if (!categoryEnumerator.MoveNext())
            {
                throw new KeyNotFoundException();
            }

            double percentDone = 0;
            double catPercentDone = 0;
            
            var currentCriteria = new Criteria(key, categoryEnumerator.Current);
            
            while (memberEnumerator.MoveNext())
            {
                if (percentDone > catPercentDone + distribution[key][categoryEnumerator.Current] )
                {
                    catPercentDone += distribution[key][categoryEnumerator.Current];
                    if (!categoryEnumerator.MoveNext())
                    {
                        throw new KeyNotFoundException();
                    }
                    currentCriteria = new Criteria(key, categoryEnumerator.Current);
                }
                memberEnumerator.Current.Criteria.Add(currentCriteria);
            }

            categoryEnumerator.Dispose();
            memberEnumerator.Dispose();
        }
        
        panel.PanelMembers = members;
        
        _repo.CreatePanel(panel);
        Console.WriteLine("Created panel " + name);
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

    public void RemovePanel(Panel panel)
    {
        _repo.DeletePanel(panel);
    }

    public void RemovePanelMember(PanelMember member)
    {
        _repo.DeletePanelMember(member);
    }
}