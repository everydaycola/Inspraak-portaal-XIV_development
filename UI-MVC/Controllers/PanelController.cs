using BL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

[RequiresOrganisation]
public class PanelController : Controller
{
    private readonly IPanelManager _manager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICriteriaManager _criteriaManager;

    public PanelController(IPanelManager manager, UserManager<ApplicationUser> userManager,
        ICriteriaManager criteriaManager)
    {
        _manager = manager;
        _userManager = userManager;
        _criteriaManager = criteriaManager;
    }

    [Authorize]
    public IActionResult MakeNewPanel()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddNewPanel(NewPanelDto newPanelDto)
    {
        string userId = _userManager.GetUserId(User);
        var createdPanel = _manager.AddPanel(
            newPanelDto.Name,
            newPanelDto.SampleRate / 100,
            CriteriaDtoCriteriaConverter(newPanelDto.Distributions),
            newPanelDto.SubRegions.Sum(subRegion => subRegion.Size),
            newPanelDto.ReservePercentage / 100,
            newPanelDto.ResponseRate / 100,
            userId
        );

        return RedirectToAction("Index", "PanelManagement", new { id = createdPanel.Id });
    }

    [HttpPost]
    public IActionResult AddDefaultPanel()
    {
        string userId = _userManager.GetUserId(User);
        var createdPanel = _manager.AddPanel(
            "Panel rond alcoholgebruik",
            0.01,
            new List<Criteria>
            {
                _criteriaManager.AddCriteria(
                    "sex",
                    "Identificeert u zich als man of vrouw?",
                    true,
                    new List<CriteriaAnswerOption>
                    {
                        _criteriaManager.AddCriteriaAnswerOption("Man", 40),
                        _criteriaManager.AddCriteriaAnswerOption("Vrouw", 60)
                    }),
                _criteriaManager.AddCriteria(
                    "leeftijd",
                    "Tot welke leeftijdscategorie behoort u?",
                    true,
                    new List<CriteriaAnswerOption>
                    {
                        _criteriaManager.AddCriteriaAnswerOption("20-29", 20),
                        _criteriaManager.AddCriteriaAnswerOption("30-39", 60),
                        _criteriaManager.AddCriteriaAnswerOption("40-49", 20),
                    })
            },
            7463,
            0.2,
            0.1,
            userId
        );
        return RedirectToAction("Index", "PanelManagement", new { id = createdPanel.Id });
    }

    private ICollection<Criteria> CriteriaDtoCriteriaConverter(ICollection<CriteriaDto> criteriaDtos)
    {
        var distributionList = new List<Criteria>();

        foreach (var crit in criteriaDtos)
        {
            var answerOptionsList = new List<CriteriaAnswerOption>();
            foreach (var answerOption in crit.AnswerOptions)
            {
                answerOptionsList.Add(_criteriaManager.AddCriteriaAnswerOption(answerOption.Option,
                    answerOption.DistributionPercentage));
            }

            distributionList.Add(_criteriaManager.AddCriteria(crit.Name, crit.Question, crit.IsDefault,
                answerOptionsList));
        }

        return distributionList;
    }
}