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
        //TODO: Actualy gather the citizencount instead of simulating this.
        PanelManagementDto pmd = new PanelManagementDto(panel.name, 20000, 0);
        pmd.PanelSize = _manager.CalculatePanelSize(pmd.CitizenCount, 0.005);
        pmd.AmountOfReserveInvites = _manager.CalculateAmountOfReserve(pmd.PanelSize, 0.2);
        pmd.TotalInvitesNeeded = _manager.CalculateTotalInvitesNeeded(pmd.PanelSize + pmd.AmountOfReserveInvites, 0.05);
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