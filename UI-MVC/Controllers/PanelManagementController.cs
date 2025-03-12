using System.Collections;
using BL;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelManagementController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;

    private readonly PanelManager _manager;
    
    
    public PanelManagementController(ILogger<PanelManagementController> logger, IManager manager)
    {
        _logger = logger;
        _manager = (PanelManager) manager;
    }

    public IActionResult Index(Guid id)
    {
        var panel = _manager.GetPanelWithRepresentationGroup(id);
        PanelManagementDto pmd = new PanelManagementDto(id, panel.Name, 20000, 0);
        pmd.PanelSize = _manager.CalculatePanelSize(pmd.CitizenCount, 0.005);
        pmd.AmountOfReserveInvites = _manager.CalculateAmountOfReserve(pmd.PanelSize, panel.RepresentationGroup.ReservePercentage);
        pmd.TotalInvitesNeeded = _manager.CalculateTotalInvitesNeeded(pmd.PanelSize + pmd.AmountOfReserveInvites, panel.RepresentationGroup.ResponseRate);
        return View(pmd);
    }
    public IActionResult LoadUniqueCodes(Guid panelId)
    {
        Panel panel = _manager.GetPanelWithPanelMembersAndCriteria(panelId);
        ICollection<UniqueCodesDto> codes = new List<UniqueCodesDto>();
        foreach (var member in panel.PanelMembers)
        {
            var codeDto = new UniqueCodesDto(member.PanelMemberId);
            foreach (var criteria in member.Criteria)
            {
                codeDto.criteria.Add(criteria.Criteria);
            }
            codes.Add(codeDto);
        }

        var model = codes
            .GroupBy(entry => entry.GroupKey) // Group by the GroupKey property in DTO
            .Select(group => new GroupedUniqueCodesDto
            {
                GroupKey = group.Key,
                Members = group.ToList()
            }).ToList();
        
        return PartialView("_UniqueCodesPartial", model);
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
}