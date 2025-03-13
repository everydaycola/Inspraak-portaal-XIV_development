using System.Collections;
using System.Text;
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

    public IEnumerable<Panel> GetAllPanels()
    {
        return _repo.ReadAllPanels();
    }

    public void AddPanel(Panel panel)
    {
        _repo.CreatePanel(panel);
    }

    //TODO: FIX NULL VALUES IN PANEL CREATION
    public Panel AddPanel(string name, int size, double sampleRate,
    Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage, double responseRate)
{
    const double tolerance = 0.0001;
    
    var panel = new Panel(name, sampleRate);
    var rpg = new RepresentationGroup(citizenCount, reservePercentage, responseRate);
    panel.RepresentationGroup = rpg;
    var rng = new Random();
    
    var panelMembers = new List<PanelMember>();
    for (int i = 0; i < size; i++)
    {
        var panelMember = new PanelMember
        {
            Panel = panel
        };
        panelMembers.Add(panelMember);
    }

    panel.PanelMembers = panelMembers;
    
    var criteriaGroups = new List<CriteriaGroup>();
    var criteriaCombinations = GenerateCriteriaCombinations(distribution);

    foreach (var combination in criteriaCombinations)
    {
        var criteriaGroup = new CriteriaGroup
        {
            Name = string.Join("-", combination.Select(c => $"{c.Value}")),
            Criteria = combination.Select(c => new Criteria(c.Key, c.Value)).ToList(),
            PanelMembers = new List<PanelMember>()
        };

        criteriaGroups.Add(criteriaGroup);
    }
    
    foreach (var criteriaGroup in criteriaGroups)
    {
        for (var i = panelMembers.Count - 1; i > 0; i--)
        {
            var k = rng.Next(i + 1);
            (panelMembers[k], panelMembers[i]) = (panelMembers[i], panelMembers[k]);
        }

        double percentDone = 0;
        foreach (var panelMember in panelMembers)
        {
            if (percentDone >= distribution[criteriaGroup.Criteria.First().Name][criteriaGroup.Criteria.First().Value])
            {
                break;
            }
            
            panelMember.CriteriaGroup = criteriaGroup;
            criteriaGroup.PanelMembers.Add(panelMember);

            percentDone += 1.0 / size;
        }
    }
    
    _repo.CreatePanel(panel);
    Console.WriteLine($"Created panel {name}");
    return panel;
}

    private List<Dictionary<string, string>> GenerateCriteriaCombinations(Dictionary<string, Dictionary<string, double>> distribution)
    {
        var keys = distribution.Keys.ToList();
    
        // Recursive function to generate combinations
        List<Dictionary<string, string>> Generate(int index)
        {
            if (index == keys.Count)
            {
                return new List<Dictionary<string, string>> { new Dictionary<string, string>() };
            }

            var key = keys[index];
            var values = distribution[key].Keys.ToList();
            var subCombinations = Generate(index + 1);

            var combinations = new List<Dictionary<string, string>>();
            foreach (var value in values)
            {
                foreach (var subCombination in subCombinations)
                {
                    var combination = new Dictionary<string, string>(subCombination)
                    {
                        [key] = value
                    };
                    combinations.Add(combination);
                }
            }

            return combinations;
        }

        return Generate(0);
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

    public string GetPanelGuidsPerGroup(Guid panelGuid)
    {
        var panel = _repo.ReadPanel(panelGuid);

        var sb = new StringBuilder();
        sb.AppendLine($"Name: {panel.Name}");
        sb.AppendLine($"Size: {panel.PanelMembers.Count}");

        var criteriaMemberPairs = panel.PanelMembers
            .SelectMany(member => member.CriteriaGroup.Criteria
                .Select(criteria => new
                {
                    MemberId = member.PanelMemberId,
                    CriterionName = criteria.Name,
                    CriterionValue = criteria.Value
                }));

        var groupedByCriteria = criteriaMemberPairs
            .GroupBy(pair => new { pair.CriterionName, pair.CriterionValue })
            .OrderBy(group => group.Key.CriterionName)
            .ThenBy(group => group.Key.CriterionValue);

        foreach (var group in groupedByCriteria)
        {
            sb.AppendLine($"Criterion: {group.Key.CriterionName}, Value: {group.Key.CriterionValue}");

            foreach (var member in group)
            {
                sb.AppendLine(member.MemberId.ToString());
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public string DescribePanel(Guid panelGuid)
    {
        var panel = _repo.ReadPanel(panelGuid);

        var sb = new StringBuilder();
        sb.AppendLine($"Name: {panel.Name}");
        sb.AppendLine($"Size: {panel.PanelMembers.Count}");

        var allCriteria = panel.PanelMembers
            .SelectMany(member => member.CriteriaGroup.Criteria
                .Select(criteria => new
                {
                    CriteriaName = criteria.Name,
                    CriteriaValue = criteria.Value
                }));

        var counts = allCriteria
            .GroupBy(c => new { c.CriteriaName, c.CriteriaValue })
            .Select(group => new
            {
                group.Key.CriteriaName,
                group.Key.CriteriaValue,
                Count = group.Count()
            })
            .OrderBy(item => item.CriteriaName)
            .ThenBy(item => item.CriteriaValue);

        foreach (var item in counts)
        {
            sb.AppendLine($"Criterion: {item.CriteriaName}, Value: {item.CriteriaValue}, Count: {item.Count}");
        }

        return sb.ToString();
    }
}