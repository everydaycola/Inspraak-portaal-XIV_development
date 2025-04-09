using BL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelController : Controller
{
    private readonly IPanelManager _manager;
    private readonly UserManager<ApplicationUser> _userManager;

    public PanelController(IPanelManager manager, UserManager<ApplicationUser> userManager)
    {
        _manager = manager;
        _userManager = userManager;
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
            newPanelDto.Size,
            newPanelDto.SampleRate/100,
            CriteriaDtoDictionaryConverter(newPanelDto.Distributions),
            GetTotalCitizenCountFromSubRegionDtos(newPanelDto.SubRegions),
            newPanelDto.ReservePercentage/100,
            newPanelDto.ResponseRate/100,
            userId
        );
        
        return RedirectToAction("Index", "PanelManagement",new { id = createdPanel.Id });
    }
    
    [HttpPost]
    public IActionResult AddDefaultPanel()
    {
        string userId = _userManager.GetUserId(User);
        var createdPanel = _manager.AddPanel(
            "Panel rond alcoholgebruik",
            150,
            0.005,
            new Dictionary<string, Dictionary<string, double>>
            {
                {
                    "sex", new Dictionary<string, double>
                    {
                        { "Man", 0.4 },
                        { "Vrouw", 0.6 }
                    }
                },
                {
                    "leeftijd", new Dictionary<string, double>
                    {
                        { "20-29", 0.2 },
                        { "30-39", 0.6 },
                        { "40-49", 0.2 }
                    }
                }
            },
            10000,
            0.2,
            0.005,
            userId = userId
        );
        return RedirectToAction("Index", "PanelManagement",new { id = createdPanel.Id });
    }

    private Dictionary<string, Dictionary<string, double>> CriteriaDtoDictionaryConverter(ICollection<CriteriaDto> criteriaDtos)
    {
        var distributionDictionary = new Dictionary<string, Dictionary<string, double>>();
        foreach (var crit in criteriaDtos)
        {
            distributionDictionary.Add(crit.Name, new Dictionary<string, double>());
            foreach (var answerOption in crit.AnswerOptions)
            {
                distributionDictionary[crit.Name].Add(answerOption.Option, answerOption.DistributionPercentage);
            }
        }
        return distributionDictionary;
    }

    private int GetTotalCitizenCountFromSubRegionDtos(ICollection<SubRegionDto> subRegionDtos)
    {
        var total = 0;
        foreach (var subRegion in subRegionDtos)
        {
            total += subRegion.Size;
        }
        return total;
    }
}