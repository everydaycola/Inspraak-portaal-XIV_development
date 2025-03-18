using System.Collections;
using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelManagementController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;

    private readonly IPanelManager _manager;
    private readonly IFileManager _fileManager;
    private readonly ICriteriaManager _criteriaManager;
    
    
    public PanelManagementController(ILogger<PanelManagementController> logger, IPanelManager manager, IFileManager fileManager, ICriteriaManager criteriaManager)
    {
        _logger = logger;
        _manager = manager;
        _fileManager = fileManager;
        _criteriaManager = criteriaManager;
    }

    public IActionResult Index(Guid id)
    {
        var panel = _manager.GetPanelWithMembersAndRepresentationGroup(id);
        IEnumerable<CriteriaGroup> criteriaGroups = _criteriaManager.GetAllCriteriaGroupForPanel(panel.Id);
        var pmd = new PanelManagementDto(id, panel.Name, 20000, 0,criteriaGroups);
        pmd.SuccesfulRegistrationCount = panel.SuccesfulRegistrationCount;
        pmd.PanelSize = _manager.CalculatePanelSize(pmd.CitizenCount, 0.005);
        pmd.AmountOfReserveInvites = _manager.CalculateAmountOfReserve(pmd.PanelSize, panel.RepresentationGroup.ReservePercentage);
        pmd.TotalInvitesNeeded = _manager.CalculateTotalInvitesNeeded(pmd.PanelSize + pmd.AmountOfReserveInvites, panel.RepresentationGroup.ResponseRate);
        pmd.IsRegistrationOpen = panel.IsRegistrationOpen;
        return View(pmd);
    }
    [HttpPost]
    public IActionResult SelectPanel(Guid panelId)
    {
        return RedirectToAction("Index", new { id = panelId });
    }
    public IActionResult PanelSelection()
    {
        var panels = _manager.GetAllPanels();
        return View(panels);
    }
    
    /*private List<GroupedUniqueCodesDto> PopulateUniqueCodesDto(Guid panelId)
    {
        var panel = _manager.GetPanelWithPanelMembersAndCriteria(panelId);
        var codes = new List<UniqueCodesDto>();
        foreach (var member in panel.PanelMembers)
        {
            if (member.CriteriaGroup == null)
            {
                var codeDto = new UniqueCodesDto(panel.Id, member.PanelMemberId, null)
                {
                    Criteria = new List<Criteria>() 
                };
                codes.Add(codeDto);
            }
            else
            {
                var codeDto = new UniqueCodesDto(panel.Id, member.PanelMemberId, member.CriteriaGroup);
                foreach (var criteriaAnswer in member.CriteriaGroup.CriteriaAnswers)
                {
                    codeDto.Criteria.Add(criteriaAnswer.criteria);
                }
                codes.Add(codeDto);
            }
        }
        
        var groupedCodes = codes.GroupBy(entry => entry.CriteriaGroup?.Id ?? Guid.Empty)
            .Select(group => new GroupedUniqueCodesDto()
            {
                GroupKey = group.Key == Guid.Empty ? "Onbekend" : group.Key.ToString(),
                Members = group.ToList(),
                Name = group.Key == Guid.Empty ? "Criteria onbekend" : group.First().CriteriaGroup?.Name,
                IsDefaultGroup = group.First().CriteriaGroup.IsADefaultGroup
            })
            .ToList();
        
        return groupedCodes.OrderByDescending(group => group.Name == "Criteria onbekend").ToList();
    }*/
    
    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }
   
}
