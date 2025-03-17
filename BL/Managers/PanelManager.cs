using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class PanelManager : IPanelManager
{
    private readonly IPanelRepository _repo;

    public PanelManager(IPanelRepository repo)
    {
        _repo = repo;
    }

    public Panel GetPanel(Guid id)
    {
        return _repo.ReadPanel(id);
    }

    public Panel GetPanelWithRepresentationGroup(Guid id)
    {
        return _repo.ReadPanelWithRepresentationGroup(id);
    }
    
    public Panel GetPanelWithMembersAndRepresentationGroup(Guid id)
    {
        return _repo.ReadPanelWithMembersAndRepresentationGroup(id);
    }

    public Panel GetPanelWithPanelMembersAndCriteria(Guid id)
    {
        return _repo.ReadPanelWithPanelMembersAndCriteria(id);
    }

    public PanelMember GetPanelByUserId(Guid memberId)
    {
        return _repo.ReadPanelByUserId(memberId);
    }

    public IEnumerable<Panel> GetAllPanels()
    {
        return _repo.ReadAllPanels();
    }

    public PanelMember GetPanelMemberById(Guid memberId)
    {
        return _repo.ReadPanelMember(memberId);
    }

    public PanelMember GetPanelMemberWithPanel(Guid id)
    {
        return _repo.ReadPanelMemberWithPanel(id);
    }

    public Panel AddPanel(string name, int size, double sampleRate,
        Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage,
        double responseRate)
    {
        // Create and initialize the panel
        var panel = new Panel(name, sampleRate)
        {
            RepresentationGroup = new RepresentationGroup(citizenCount, reservePercentage, responseRate)
        };
        
        // Generate and shuffle panel members
        var random = new Random();
        panel.PanelMembers = Enumerable.Range(0, size)
            .Select(_ => new PanelMember(panel))
            .ToList()
            .OrderBy(_ => random.Next())
            .ToList();

        // Create criteria list from distribution using LINQ
        var groupedCriteria = distribution
            .Select(outer => new Criteria(outer.Key)
            {
                Values = outer.Value
                    .Select(inner => new CriteriaValue(inner.Key, inner.Value))
                    .ToList()
            })
            .ToList()
            .ToDictionary(c => c.Name);
        
        var criteriaCombinations = GenerateCriteriaCombinations(groupedCriteria);

        // Create criteria groups from the generated combinations
        var criteriaGroups = CreateCriteriaGroups(criteriaCombinations, panel);
        
        // Persist criteria groups to the repository
        foreach (var group in criteriaGroups)
        {
            _repo.CreateCriteriaGroup(group);
        }

        return panel;
    }
    
    private List<Dictionary<string, CriteriaValue>> GenerateCriteriaCombinations(Dictionary<string,Criteria> groupedCriteria)
    {
        var combinations = new List<Dictionary<string, CriteriaValue>>();

        void GenerateCombinations(Dictionary<string, CriteriaValue> current, List<string> remainingCriteria)
        {
            if (remainingCriteria.Count == 0)
            {
                combinations.Add(new Dictionary<string, CriteriaValue>(current));
                return;
            }

            var nextCriterionName = remainingCriteria[0];
            var nextCriteriaValues = groupedCriteria[nextCriterionName].Values;

            foreach (var value in nextCriteriaValues)
            {
                current[nextCriterionName] = value;
                GenerateCombinations(current, remainingCriteria.Skip(1).ToList());
                current.Remove(nextCriterionName);
            }
        }

        GenerateCombinations(new Dictionary<string, CriteriaValue>(), groupedCriteria.Keys.ToList());
        return combinations;
    }
    
    private List<CriteriaGroup> CreateCriteriaGroups(List<Dictionary<string, CriteriaValue>> combinations, Panel panel)
    {
        var criteriaGroups = new List<CriteriaGroup>();
        var currentUserIndex = 0;

        foreach (var combo in combinations)
        {
            var comboKey = string.Join("-", combo.Values.Select(v => v.Value));
            var comboPercentage = combo.Values.Aggregate(1.0, (acc, v) => acc * v.DistributionPercentage);
            var totalMembersNeeded = (int)Math.Round(comboPercentage * panel.PanelMembers.Count);

            if (currentUserIndex + totalMembersNeeded > panel.PanelMembers.Count)
            {
                totalMembersNeeded = panel.PanelMembers.Count - currentUserIndex;
            }

            var assignedMembers = panel.PanelMembers
                .Skip(currentUserIndex)
                .Take(totalMembersNeeded)
                .ToList();
            currentUserIndex += totalMembersNeeded;

            criteriaGroups.Add(new CriteriaGroup(comboKey, assignedMembers));
        }

        return criteriaGroups;
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
        //basePanelSize = calculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of response to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }

    public void UpdatePanel(Guid id, bool isRegistrationOpen)
    {
        var panel = _repo.ReadPanel(id);
        if (panel == null) throw new NullReferenceException("Panel with id: " + id + " was not found.");
        panel.IsRegistrationOpen = isRegistrationOpen;
        _repo.UpdatePanel(panel);
    }

    public void UpdatePanelRegistrationCount(Guid id, bool increase)
    {
        var panel = _repo.ReadPanel(id);
        if (panel != null)
        {
            if (increase)
            {
                panel.SuccesfulRegistrationCount++;
            }
            else
            {
                panel.SuccesfulRegistrationCount--;
            }

            _repo.UpdatePanel(panel);
            return;
        }

        throw new NullReferenceException("Panel with id: " + id + " was not found");
    }

    public PanelMember UpdatePanelMember(PanelMember member)
    {
        return _repo.UpdatePanelMember(member);
    }
}