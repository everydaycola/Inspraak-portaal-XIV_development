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

    public void AddPanel(Panel panel)
    {
        _repo.CreatePanel(panel);
    }

    public Panel AddPanel(string name, int size, double sampleRate, Dictionary<string, Dictionary<string, double>> distribution)
    {
        const double tolerance = 0.0001;
        var panel = new Panel(name, sampleRate);
        var members = new List<PanelMember>();
        for (int i = 0; i < size; i++)
        {
            PanelMember newMember = new PanelMember();
            newMember.Panel = panel;
            newMember.Criteria = new List<PanelMemberCriteria>();
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
                if (percentDone > catPercentDone + distribution[key][categoryEnumerator.Current] - tolerance )
                {
                    catPercentDone += distribution[key][categoryEnumerator.Current];
                    if (categoryEnumerator.MoveNext())
                    {
                        currentCriteria = new Criteria(key, categoryEnumerator.Current);
                    }
                }

                var panelMemberCriteria = new PanelMemberCriteria(memberEnumerator.Current, currentCriteria);
                memberEnumerator.Current.Criteria.Add(panelMemberCriteria);
                currentCriteria.PanelMembers.Add(panelMemberCriteria);

                percentDone += (double) 1 / size;
            }

            categoryEnumerator.Dispose();
            memberEnumerator.Dispose();
        }
        
        panel.PanelMembers = members;

        _repo.CreatePanel(panel);
        Console.WriteLine("Created panel " + name);
        return panel;
    }
    
    public int CalculatePanelSize(int citizenCount, double samplePercentage)
    {
        //CitizenCount = amount of citizens in gemeente.
        return (int)(citizenCount * samplePercentage);
    }
    public int CalculateAmountOfReserve(int panelSize, double samplePercentage)
    {
        //panelSize = calculatedByCalculatePanelSize
        return (int) (panelSize * samplePercentage);
    }
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate)
    { 
        //basePanelSize = claculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of resposne to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }
}