using BL;
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
        PanelManagementDto pmd = new PanelManagementDto(panel.Id,panel.Name, 20000, 0);
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

    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }
}