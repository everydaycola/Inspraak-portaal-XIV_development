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
    
    
    public PanelManagementController(ILogger<PanelManagementController> logger, IPanelManager manager)
    {
        _logger = logger;
        _manager = manager;
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
            var codeDto = new UniqueCodesDto(panel.Id, member.PanelMemberId, member.CriteriaGroup);
            foreach (var criteria in member.CriteriaGroup.Criteria)
                {
                    codeDto.criteria.Add(criteria);
                }
            codes.Add(codeDto);
        }

        var model = codes
            .GroupBy(entry => entry.CriteriaGroup.Id) 
            .Select(group => new GroupedUniqueCodesDto
            {
                GroupKey = group.Key.ToString(),
                Members = group.ToList(),
                Name = group.First().CriteriaGroup.Name
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