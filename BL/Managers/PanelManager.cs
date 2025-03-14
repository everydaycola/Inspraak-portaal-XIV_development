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

    public void AddPanel(Panel panel)
    {
        _repo.CreatePanel(panel);
    }
    
    
    
    public Panel AddPanel(string name, int size, double sampleRate,
        Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage,
        double responseRate)
    {
        var panel = new Panel(name, sampleRate);
        var rpg = new RepresentationGroup(citizenCount, reservePercentage, responseRate);
        panel.RepresentationGroup = rpg;
        
        panel.PanelMembers = GeneratePanelMembers(size, panel);
        var random = new Random();
        panel.PanelMembers = panel.PanelMembers.OrderBy(_ => random.Next()).ToList();
        //PrintDistribution(distribution);

        List<Criteria> criteriaList = new List<Criteria>();
        
        foreach (var outerEntry in distribution)
        {
            string outerKey = outerEntry.Key;
            var innerDict = outerEntry.Value;
            foreach (var innerEntry in innerDict)
            {
                string innerKey = innerEntry.Key;
                double value = innerEntry.Value;
                criteriaList.Add(new Criteria(outerKey, innerKey, value));
            }
        }
        //GROUP CRITERIA BY NAME
        var groupedCriteria = criteriaList
            .GroupBy(c => c.Name)
            .ToDictionary(g => g.Key, g => g.ToList());
        
        //GENERATE COMBINATIONS
        List<Dictionary<string, Criteria>> combinations = new List<Dictionary<string, Criteria>>();
        void GenerateCombinations(Dictionary<string, Criteria> current, List<string> remainingCriteria)
        {
            if (remainingCriteria.Count == 0)
            {
                combinations.Add(new Dictionary<string, Criteria>(current));
                return;
            }
            string nextCriterion = remainingCriteria[0];
            var nextCriteriaValues = groupedCriteria[nextCriterion];

            foreach (var value in nextCriteriaValues)
            {
                current[nextCriterion] = value;
                GenerateCombinations(current, remainingCriteria.Skip(1).ToList());
                current.Remove(nextCriterion);
            }
        }
        
        GenerateCombinations(new Dictionary<string, Criteria>(), groupedCriteria.Keys.ToList());

        List<CriteriaGroup> criteriaGroups = new List<CriteriaGroup>();
        int currentuserIndex = 0;
        foreach (var combo in combinations)
        {
            string comboKey = string.Join("-", combo.Values.Select(c => c.Value));
            double comboPercentage = combo.Values.Aggregate(1.0, (acc, c) => acc * c.distributionPercentage);
            int totalMembersNeeded = (int)Math.Round(comboPercentage * panel.PanelMembers.Count);
            if (currentuserIndex + totalMembersNeeded > panel.PanelMembers.Count)
            {
                totalMembersNeeded = panel.PanelMembers.Count - currentuserIndex;
            }
            
            var assignedMembers = panel.PanelMembers
                .Skip(currentuserIndex)
                .Take(totalMembersNeeded)
                .ToList();
            currentuserIndex += totalMembersNeeded;
            
            var criteriaGroup = new CriteriaGroup(comboKey, assignedMembers);
            criteriaGroups.Add(criteriaGroup);
        }
        
        foreach (var group in criteriaGroups)
        {
            _repo.CreateCriteriaGroup(group);
        }
        
        return panel;
    }
    
    private ICollection<PanelMember> GeneratePanelMembers(int size, Panel panel)
    {
        var panelMembers = new List<PanelMember>();
        for (int i = 0; i < size; i++)
        {
            var panelMember = new PanelMember
            {
                Panel = panel
            };
            panelMembers.Add(panelMember);
        }

        return panelMembers;
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

    public void UpdatePanel(Guid id, bool isRegistrationOpen)
    {
        var panel = _repo.ReadPanel(id);
        if (panel != null)
        {
            panel.IsRegistrationOpen = isRegistrationOpen;
            _repo.UpdatePanel(panel);
            return;
        }

        throw new NullReferenceException("Panel with id: " + id + " was not found.");
    }

}
