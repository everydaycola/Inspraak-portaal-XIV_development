using System.Text;
using BL;

namespace UI_CA.ca_helpers;

public class PanelCaHelper
{
    public readonly PanelManager _manager;
    public PanelCaHelper(PanelManager panelManager)
    {
        _manager = panelManager;
    }
    
    public string GetPanelGuidsPerGroup(Guid panelGuid)
    {
        var panel = _manager.GetPanel(panelGuid);

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
        var panel = _manager.GetPanel(panelGuid);
        
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