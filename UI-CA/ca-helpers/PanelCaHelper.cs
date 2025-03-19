using System.Text;
using BL.Interfaces;

namespace UI_CA.ca_helpers;

public class PanelCaHelper
{
    private readonly IPanelManager _manager;

    public PanelCaHelper(IPanelManager panelManager)
    {
        _manager = panelManager;
    }

    // Updated GetPanelGuidsPerGroup method
    public string GetPanelGuidsPerGroup(Guid panelGuid)
    {
        var panel = _manager.GetPanel(panelGuid);

        var sb = new StringBuilder();
        sb.AppendLine($"Name: {panel.Name}");
        sb.AppendLine($"Size: {panel.PanelMembers.Count}");

        // Extract member IDs and their associated criteria answers
        var criteriaMemberPairs = panel.PanelMembers
            .SelectMany(member => member.CriteriaGroup.CriteriaAnswers
                .Select(answer => new
                {
                    MemberId = member.PanelMemberId,
                    CriterionName = answer.Criteria.Name,
                    CriterionValue = answer.CriteriaValue.Value
                }));

        // Group by criterion name and value
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

    // Updated DescribePanel method
    public string DescribePanel(Guid panelGuid)
    {
        var panel = _manager.GetPanel(panelGuid);

        var sb = new StringBuilder();
        sb.AppendLine($"Name: {panel.Name}");
        sb.AppendLine($"Size: {panel.PanelMembers.Count}");

        // Extract all criteria answers from members
        var allCriteria = panel.PanelMembers
            .SelectMany(member => member.CriteriaGroup.CriteriaAnswers
                .Select(answer => new
                {
                    CriteriaName = answer.Criteria.Name,
                    CriteriaValue = answer.CriteriaValue.Value
                }));

        // Group by criterion name and value, and count occurrences
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
