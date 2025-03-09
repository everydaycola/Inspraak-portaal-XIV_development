using System.Text;

namespace Domain.CitizenPanel;

public class Panel
{
    public Guid Id { get; set; }
    public string name { get; set; }
    public ICollection<PanelMember> PanelMembers { get; set; }

    public Panel(string name)
    {
        this.name = name;
    }

    public string getGuidsPerGroup()
    {
        // todo: doesnt cross reference, guid's get printed multiple times. 
        var sb = new StringBuilder();
        sb.AppendLine("name: " + name);
        sb.AppendLine("size: " + PanelMembers.Count);
        // Group panel members by criteria and collect GUIDs
        var criteriaGuids = PanelMembers
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

    public string describe()
    {
        var sb = new StringBuilder();
        sb.AppendLine("name: " + name);
        sb.AppendLine("size: " + PanelMembers.Count);
        var counts = PanelMembers
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