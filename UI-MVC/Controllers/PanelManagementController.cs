using System.Collections;
using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelManagementController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;

    private readonly IPanelManager _manager;
    private readonly IFileManager _fileManager;
    private readonly ICriteriaManager _criteriaManager;
    private readonly ICalculationManager _calcManager;
    
    
    public PanelManagementController(ILogger<PanelManagementController> logger, IPanelManager manager, IFileManager fileManager, ICriteriaManager criteriaManager, ICalculationManager calcHelper)
    {
        _logger = logger;
        _manager = manager;
        _fileManager = fileManager;
        _criteriaManager = criteriaManager;
        _calcManager = calcHelper;
    }

    public IActionResult Index(Guid id)
    {
        var panelMembers = _manager.GetPanelMembersAndRepresentationGroup(id);
        var panel = panelMembers.First().Panel;
        var pmd = new PanelManagementDto(id, panel.Name, 20000, 0);
        pmd.SuccesfulRegistrationCount = panel.SuccesfulRegistrationCount;
        pmd.PanelSize = _calcManager.CalculatePanelSize(pmd.CitizenCount, 0.005);
        pmd.AmountOfReserveInvites = _calcManager.CalculateAmountOfReserve(pmd.PanelSize, panel.RepresentationGroup.ReservePercentage);
        pmd.TotalInvitesNeeded = _calcManager.CalculateTotalInvitesNeeded(pmd.PanelSize + pmd.AmountOfReserveInvites, panel.RepresentationGroup.ResponseRate);
        pmd.IsRegistrationOpen = panel.IsRegistrationOpen;
        pmd.ExtraCriteriaViewModel.CriteriaMemberCount = _calcManager.CalculateAllCriteriaCountForPanel(panel.Id);
        pmd.ExtraCriteriaViewModel.SuccesfulRegistrationCount = _calcManager.CalculateSuccesfulRegistrationCount(panel.Id);
        pmd.ExtraCriteriaViewModel.uniqueCodesDto = new uniqueCodesDto
        {
            panelId = panel.Id, 
            panelMembers = _criteriaManager.GetPanelMembersGroupedByResponsesForDefaultCriteria(panel.Id)
        };
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
    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }
   
}
