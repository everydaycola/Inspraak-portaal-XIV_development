using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelManagementController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;

    private readonly IPanelManager _manager;
    private readonly ICriteriaManager _criteriaManager;
    private readonly ICalculationManager _calcManager;
    private readonly UserManager<IdentityUser> _userManager;


    public PanelManagementController(ILogger<PanelManagementController> logger, IPanelManager manager, ICriteriaManager criteriaManager, ICalculationManager calcHelper, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _manager = manager;
        _criteriaManager = criteriaManager;
        _calcManager = calcHelper;
        _userManager = userManager;
    }

    public IActionResult Index(Guid id)
    {
        var panel = _manager.GetPanelWithRepresentationGroup(id);
        var panelSize = _calcManager.CalculatePanelSize(panel.RepresentationGroup.CitizenCount, 0.005);
        var amountOfReserveInvites =
            _calcManager.CalculateAmountOfReserve(panelSize, panel.RepresentationGroup.ReservePercentage);
        return View(new PanelManagementDto
        {
            PanelId = id,
            PanelName = panel.Name,
            CitizenCount = panel.RepresentationGroup.CitizenCount,
            PanelSize = panelSize,
            AmountOfReserveInvites = amountOfReserveInvites,
            TotalInvitesNeeded = _calcManager.CalculateTotalInvitesNeeded(panelSize + amountOfReserveInvites, panel.RepresentationGroup.ResponseRate),
            IsRegistrationOpen = panel.IsRegistrationOpen,
            PlanningGroupMembers = _manager.GetAllPlanningGroupMembersWithIdentityUserForPanel(panel.Id),
            ExtraCriteriaViewModel = new ExtraCriteriaViewModel
            {
                CriteriaMemberCount = _criteriaManager.GetAllCriteriaCountsGroupedByValue(panel.Id),
                SuccesfulRegistrationCount = panel.SuccessfulRegistrationCount,
            },
            UniqueCodesDto = new uniqueCodesDto
            {
                panelId = panel.Id,
                panelMembers = _criteriaManager.GetPanelMembersGroupedByResponsesForDefaultCriteria(panel.Id)
            }
        });
    }
    [HttpPost]
    public IActionResult SelectPanel(Guid panelId)
    {
        return RedirectToAction("Index", new { id = panelId });
    }
    [Authorize]
    public IActionResult PanelSelection()
    {
        string userId = _userManager.GetUserId(User);
        var panels = _manager.GetAllPanelsOwnedBy(userId);
        return View(panels);
    }
    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }
   
}
