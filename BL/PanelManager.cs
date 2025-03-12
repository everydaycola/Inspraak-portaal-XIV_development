using System.Text;
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

    public Panel AddPanel(string name, int size, double sampleRate, Dictionary<string, Dictionary<string, double>> distribution,int citizenCount, double reservePercentage, double responseRate )
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
        panel.RepresentationGroup = new RepresentationGroup(citizenCount,reservePercentage,responseRate);

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
    
    public string getPanelGuidsPerGroup(Guid guid)
    {
        // todo: doesnt cross reference, guid's get printed multiple times. 
        var panel = _repo.ReadPanel(guid);
        var sb = new StringBuilder();
        sb.AppendLine("name: " + panel.Name);
        sb.AppendLine("size: " + panel.PanelMembers.Count);
        // Group panel members by criteria and collect GUIDs
        var criteriaGuids = panel.PanelMembers
            .SelectMany(member => 
                member.Criteria.Select(criterion => new { MemberId = member.PanelMemberId, Criterion = criterion }))
            .GroupBy(item => new { item.Criterion.Criteria.Name, item.Criterion.Criteria.Value })
            .Select(group => new
            {
                CriterionName = group.Key.Name,
                CriterionValue = group.Key.Value,
                MemberGuids = string.Join(Environment.NewLine, group.Select(item => item.MemberId))
            })
            .OrderBy(item => item.CriterionName);

        // Print the results
        foreach (var item in criteriaGuids)
        {
            sb.AppendLine($"Criterion: {item.CriterionName}, Value: {item.CriterionValue}");
            sb.AppendLine(item.MemberGuids);
            sb.AppendLine(); // Add a blank line for readability
        }

        return sb.ToString();
    }

    public string describePanel(Guid guid)
    {
        var panel = _repo.ReadPanel(guid);
        var sb = new StringBuilder();
        sb.AppendLine("name: " + panel.Name);
        sb.AppendLine("size: " + panel.PanelMembers.Count);
        var counts = panel.PanelMembers
            // puts all criteria in one big list
            .SelectMany(m => m.Criteria)
            // groups by criteria objects
            .GroupBy(c => c.Criteria)
            .Select(group => new
            {
                CriteriaName = group.Key.Name,
                CriteriaValue = group.Key.Value,
                Count = group.Count()
            })
            .OrderBy(item => item.CriteriaValue)
            .ThenBy(item => item.CriteriaName);


        sb.AppendLine(String.Join("\n",
            counts.Select(item =>
                $"Criterion: {item.CriteriaName}, Value: {item.CriteriaValue}, Count: {item.Count}")));

        return sb.ToString();
    }
}