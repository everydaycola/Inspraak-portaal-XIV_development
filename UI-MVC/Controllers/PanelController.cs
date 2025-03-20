using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelController : Controller
{
    private readonly IPanelManager _manager;

    public PanelController(IPanelManager manager)
    {
        _manager = manager;
    }

    public IActionResult MakeNewPanel()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult AddNewPanel(NewPanelDto newPanelDto)
    {
        var createdPanel = _manager.AddPanel(
            newPanelDto.Name,
            newPanelDto.Size,
            newPanelDto.SampleRate,
            newPanelDto.Distributions,
            newPanelDto.CitizenCount,
            newPanelDto.ReservePercentage,
            newPanelDto.ResponseRate
        );
        
        return RedirectToAction("Index", "PanelManagement",new { id = createdPanel.Id });
    }
    
    [HttpPost]
    public IActionResult AddDefaultPanel()
    {
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
            0.005
        );
        return RedirectToAction("Index", "PanelManagement",new { id = createdPanel.Id });
    }

}