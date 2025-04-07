using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelController : Controller
{
    private readonly IPanelManager _manager;
    private readonly UserManager<IdentityUser> _userManager;

    public PanelController(IPanelManager manager, UserManager<IdentityUser> userManager)
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
            newPanelDto.SampleRate,
            CriteriaDtoDictionaryConverter(newPanelDto.Distributions),
            newPanelDto.CitizenCount,
            newPanelDto.ReservePercentage,
            newPanelDto.ResponseRate,
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
                        { "m", 0.4 },
                        { "v", 0.6 }
                    }
                },
                {
                    "leef", new Dictionary<string, double>
                    {
                        { "20", 0.2 },
                        { "30", 0.6 },
                        { "40", 0.2 }
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
}