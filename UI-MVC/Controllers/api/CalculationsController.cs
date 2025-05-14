using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto.ApiDtos;
namespace UI_MVC.Controllers.api;


[ApiController]
[Route("api/[controller]")]
public class CalculationsController : ControllerBase
{
    private readonly ICalculationManager _calculationManager;
    public CalculationsController(ICalculationManager calculationManager)
    {
        _calculationManager = calculationManager;
    }
    
    [HttpGet("panelsize")]
    public ActionResult<int> GetPanelSize([FromQuery] int citizenCount, [FromQuery] double samplePercentage )
    {
        var panelSize = _calculationManager.CalculatePanelSize(citizenCount, samplePercentage);
        return Ok(panelSize);
    }
    
    [HttpGet("reservesize")]
    public ActionResult<int> GetAmountOfReserve([FromQuery] int citizenCount, [FromQuery] double reservePercentage )
    {
        var reserveCount = _calculationManager.CalculateAmountOfReserve(citizenCount, reservePercentage);
        return Ok(reserveCount);
    }
    
    [HttpGet("totalinvites")]
    public ActionResult<int> GetAmountOfInvitesNeeded([FromBody] TotalInvitesRequestDto request)
    {
        if (request == null)
            return BadRequest("Request body is missing.");

        var totalInvites = _calculationManager.CalculateTotalInvitesNeeded(request.CitizenCount, request.SampleRate);
        return Ok(totalInvites);
    }
    
}