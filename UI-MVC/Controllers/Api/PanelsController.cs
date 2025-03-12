using BL;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers.Api;

[ApiController]
[Route("/api/panels")]
public class PanelsController : Controller
{
    private readonly PanelManager _manager;

    public PanelsController(IManager manager)
    {
        _manager = (PanelManager) manager;
    }
    
    [HttpPost]
    public IActionResult AddNewSupplier(NewPanelDto newPanelDto)
    {
        var CreatedPanel = _manager.AddPanel(
            newPanelDto.Name,
            newPanelDto.Size,
            newPanelDto.SampleRate,
            newPanelDto.Distributions,
            newPanelDto.CitizenCount,
            newPanelDto.ReservePercentage,
            newPanelDto.ResponseRate
        );

        //todo: redirect to a page with detail about the just created panel
        return Created();
    }
}