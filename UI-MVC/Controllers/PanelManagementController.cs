using BL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Models.Dto;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;
[RequiresOrganisation]
public class PanelManagementController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;

    private readonly IPanelManager _manager;
    private readonly ICriteriaManager _criteriaManager;
    private readonly ICalculationManager _calcManager;
    private readonly UserManager<ApplicationUser> _userManager;


    public PanelManagementController(ILogger<PanelManagementController> logger, IPanelManager manager, IFileManager fileManager, ICriteriaManager criteriaManager, ICalculationManager calcHelper, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _manager = manager;
        _criteriaManager = criteriaManager;
        _calcManager = calcHelper;
        _userManager = userManager;
    }

    public IActionResult Index(Guid id)
    {
        if (id == Guid.Empty)
        {
            return RedirectToAction("PanelSelection");
        }
        var panel = _manager.GetPanelWithRepresentationGroup(id);
        var panelSize = _calcManager.CalculatePanelSize(panel.RepresentationGroup.CitizenCount, panel.SampleRate);
        var amountOfReserveInvites =
            _calcManager.CalculateAmountOfReserve(panelSize, panel.RepresentationGroup.ReservePercentage);
        var criteriaList = _criteriaManager.GetAllDesiredCriteriaPercentages(panel.Id).ToList();
        return View(new PanelManagementViewModel
        {
            PanelId = id,
            CitizenCount = panel.RepresentationGroup.CitizenCount,
            PanelSize = panelSize,
            AmountOfReserveInvites = amountOfReserveInvites,
            TotalInvitesNeeded = _calcManager.CalculateTotalInvitesNeeded(panelSize + amountOfReserveInvites, panel.RepresentationGroup.ResponseRate),
            IsRegistrationOpen = panel.IsRegistrationOpen,
            AnyCrossCriteria = criteriaList.Any(c => c.IsDistributionKnown),
            AnyUnknownCriteria = criteriaList.Any(c => !c.IsDistributionKnown),
            ExtraCriteriaDto = new ExtraCriteriaDto
            {
                CriteriaGroupAbsoluteMemberCount = _manager.CalculateCrossDistributionAbsolute(panel.Id),
                CriteriaMemberCount = _criteriaManager.GetAllCriteriaCountsGroupedByValue(panel.Id, onlyUnknown: true),
                Criteria = criteriaList,
                SuccessfulRegistrationCount = panel.SuccessfulRegistrationCount,
                DesiredRegistrationCount = panelSize,
            },
        });
    }
    public IActionResult People(Guid id)
    {
        if (id == Guid.Empty)
        {
            return RedirectToAction("PanelSelection");
        }
        var panel = _manager.GetPanelWithRepresentationGroup(id);
        var panelSize = _calcManager.CalculatePanelSize(panel.RepresentationGroup.CitizenCount, panel.SampleRate);
        var criteriaList = _criteriaManager.GetAllDesiredCriteriaPercentages(panel.Id).ToList();
        var amountOfReserveInvites =
            _calcManager.CalculateAmountOfReserve(panelSize, panel.RepresentationGroup.ReservePercentage);
        
        return View(new PeopleManagementViewModel
        {
            PanelId = panel.Id,
            UniqueCodesDto = new uniqueCodesDto
            {
                panelId = panel.Id,
                panelMembers = _criteriaManager.GetPanelMembersGroupedByResponsesForDefaultCriteriaGroupedByPhase(panel.Id),
                Phases = panel.LastPhase
            },
            PlanningGroupMembers = _manager.GetAllPlanningGroupMembersWithIdentityUserForPanel(panel.Id),
            ExtraCriteriaDto = new ExtraCriteriaDto
            {
                CriteriaGroupAbsoluteMemberCount = _manager.CalculateCrossDistributionAbsolute(panel.Id),
                CriteriaMemberCount = _criteriaManager.GetAllCriteriaCountsGroupedByValue(panel.Id, onlyUnknown: true),
                Criteria = criteriaList,
                SuccessfulRegistrationCount = panel.SuccessfulRegistrationCount,
                DesiredRegistrationCount = panelSize,
            },
            IsRegistrationOpen = panel.IsRegistrationOpen,
            PanelSize = panelSize,
            AnyCrossCriteria = criteriaList.Any(c => c.IsDistributionKnown),
            AnyUnknownCriteria = criteriaList.Any(c => !c.IsDistributionKnown),
            AmountOfReserveInvites = amountOfReserveInvites,
            TotalInvitesNeeded = _calcManager.CalculateTotalInvitesNeeded(panelSize + amountOfReserveInvites, panel.RepresentationGroup.ResponseRate)
        });
    }
    
    [HttpPost]
    public IActionResult SelectPanel(Guid panelId)
    {
        return RedirectToAction("Index", new { id = panelId });
    }
    
    [HttpPost]
    public IActionResult NewPhase(Guid guid, double newResponseRate)
    {
        _manager.NewPanelPhase(guid, newResponseRate / 100);
        
        return RedirectToAction("Index", new { id = guid });
    }

    [HttpPost]
    public IActionResult AddPlanningGroupmember(PlanningGroupMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", model.PanelId);
        }
        _manager.AddPlanningsGroupMember(model.PanelId,model.Email, model.Naam, model.Functie);
        return RedirectToAction("Index", model.PanelId);
    }

    public IActionResult DeletePlanningsGroupmember(Guid panelId, Guid planningsGroupMemberId)
    {
        _manager.DeletePlanningsGroupmember(planningsGroupMemberId);
        return RedirectToAction("Index", panelId);
    }
    
    [Authorize]
    public IActionResult PanelSelection()
    {
        var panels = _manager.GetAllPanels().ToList(); // Materialize the collection
        if (panels.Count == 1)
        {
            return RedirectToAction("Index", new { id = panels[0].Id });
        }
        return View(panels);
    }
    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }

    public IActionResult EndRegistration(Guid panelId)
    {
        var allDesiredCriteriaPercentages = _criteriaManager.GetAllDesiredCriteriaPercentages(panelId);

        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        _manager.EndRegistration(panelId, allDesiredCriteriaPercentages, true,baseUrl);

        return RedirectToAction("Index", new { id = panelId });
    }
}