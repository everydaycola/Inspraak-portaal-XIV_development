using BL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.ViewModels;

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
    public IActionResult AddNewPanel(NewPanelViewModel newPanelViewModel)
    {
        string userId = _userManager.GetUserId(User);
        
        var createdPanel = _manager.AddPanel(
            newPanelViewModel.Name,
            newPanelViewModel.SampleRate / 100,
            CreateDistributionList(newPanelViewModel.SubRegions, CriteriaDtoCriteriaConverter(newPanelViewModel.Distributions)),
            newPanelViewModel.SubRegions.Sum(subRegion => subRegion.Size),
            newPanelViewModel.ReservePercentage / 100,
            newPanelViewModel.ResponseRate / 100,
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
                    }, true),
                _criteriaManager.AddCriteria(
                    "leeftijd",
                    "Tot welke leeftijdscategorie behoort u?",
                    true,
                    new List<CriteriaAnswerOption>
                    {
                        _criteriaManager.AddCriteriaAnswerOption("20-29", 20),
                        _criteriaManager.AddCriteriaAnswerOption("30-39", 60),
                        _criteriaManager.AddCriteriaAnswerOption("40-49", 20),
                    }, true)
            },
            7463,
            0.2,
            0.1,
            userId
        );
        return RedirectToAction("Index", "PanelManagement", new { id = createdPanel.Id });
    }

    private ICollection<Criteria> CriteriaDtoCriteriaConverter(ICollection<CriteriaViewModel> criteriaDtos)
    {
        var distributionList = new List<Criteria>();

        foreach (var crit in criteriaDtos)
        {
            var answerOptionsList = new List<CriteriaAnswerOption>();
            foreach (var answerOption in crit.AnswerOptions)
            {
                if (crit.IsDistributionKnown)
                {
                    answerOptionsList.Add(_criteriaManager.AddCriteriaAnswerOption(answerOption.Option,
                        double.Parse(answerOption.DistributionPercentage.Replace(".", ","))));
                }
                else
                {
                    answerOptionsList.Add(_criteriaManager.AddCriteriaAnswerOption(answerOption.Option, 0));
                }
            }

            distributionList.Add(_criteriaManager.AddCriteria(crit.Name, crit.Question, crit.IsDefault,
                answerOptionsList, crit.IsDistributionKnown));
        }

        return distributionList;
    }

    private Criteria CreateRegionCriteria(ICollection<SubRegionDto> subRegionDtos)
    {
        ICollection<CriteriaAnswerOption> regionsDistribution = new List<CriteriaAnswerOption>();
        double totalPopulation = subRegionDtos.Sum(subRegionDto => subRegionDto.Size);
        foreach (var subRegionDto in subRegionDtos)
        {
            var distributionPercentage = double.Round(subRegionDto.Size / totalPopulation, 4);
            regionsDistribution.Add(
                _criteriaManager.AddCriteriaAnswerOption(subRegionDto.Name, distributionPercentage * 100));
        }

        var regionCriteria = _criteriaManager.AddCriteria(
            "Area", //Area because the criteria are sorted alphabetically later on
            "In welke (deel)gemeente of wijk woont u?",
            true,
            regionsDistribution,
            true
        );
        return regionCriteria;
    }

    private ICollection<Criteria> CreateDistributionList(ICollection<SubRegionDto> subRegionDtos, ICollection<Criteria> criteria)
    {
        var distributionList = new List<Criteria>();
        if (subRegionDtos.Count > 1)
        {
            distributionList.Add(CreateRegionCriteria(subRegionDtos));
        }
        foreach (var crit in criteria)
        {
            distributionList.Add(crit);
        }
        return distributionList;
    }
}