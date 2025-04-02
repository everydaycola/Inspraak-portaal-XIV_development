using System.Text;
using BL.Interfaces;

namespace UI_CA.ca_helpers;

public class PanelCaHelper
{
    private readonly IPanelManager _manager;
    private readonly ICalculationManager _calcManager;
    private readonly ICriteriaManager _critManager;

    public PanelCaHelper(IPanelManager panelManager, ICriteriaManager criteriaManager, ICalculationManager calculationManager)
    {
        _manager = panelManager;
        _critManager = criteriaManager;
        _calcManager = calculationManager;
    }

    // Updated GetPanelGuidsPerGroup method
    public string GetPanelGuidsPerGroup(Guid panelGuid)
    {
        var panel = _manager.GetPanel(panelGuid);

        var sb = new StringBuilder();
        sb.AppendLine($"Name: {panel.Name}");
        sb.AppendLine($"Size: {_calcManager.CalculateAmountOfMembersInPanel(panel.Id)}");


        var criteriaMemberPairs = _critManager.GetPanelMembersGroupedByResponsesForDefaultCriteria(panel.Id);
        
        foreach (var group in criteriaMemberPairs)
        {
            sb.AppendLine($"Criterion: {group.Key}");

            foreach (var member in group.Value)
            {
                sb.AppendLine(member.PanelMemberId.ToString());
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
        sb.AppendLine($"Size: {_calcManager.CalculateAmountOfMembersInPanel(panel.Id)}");

        var counts = _calcManager.CalculateAllCriteriaCountForPanel(panel.Id);

        foreach (var item in counts)
        {
            sb.AppendLine($"Criterion: {item.Key},Count: {item.Value}");
        }

        return sb.ToString();
    }
}
