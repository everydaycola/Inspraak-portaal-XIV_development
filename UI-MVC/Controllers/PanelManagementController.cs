using BL;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelManagementController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly PanelManager _manager;
    
    
    public PanelManagementController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = (PanelManager) manager;
    }

    public IActionResult Index(Guid id)
    {
        var panel = _manager.GetPanelWithRepresentationGroup(id);
        int totalMemberCount = (int)(panel.RepresentationGroup.memberCount +
                                     (panel.RepresentationGroup.memberCount * panel.RepresentationGroup.reservePercentage));
        PanelManagementDto pmd = new PanelManagementDto(panel.name, totalMemberCount, 0);
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
}