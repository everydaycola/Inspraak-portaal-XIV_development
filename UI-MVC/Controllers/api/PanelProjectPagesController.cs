using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class PanelProjectPagesController : ControllerBase
{
    private readonly IPanelProjectPageManager _projectPageManager;

    public PanelProjectPagesController(IPanelProjectPageManager projectPageManager)
    {
        _projectPageManager = projectPageManager;
    }

    [HttpPost ("toggleVisibility")]
    public async Task<IActionResult> ToggleSuggestionGlobalVisibility([FromQuery] string suggestionId)
    {
        var suggestionGuid = Guid.Parse(suggestionId);
        _projectPageManager.ChangeSuggestionVisibility(suggestionGuid);
        
        return Ok(new { success = true});
    }

    [HttpGet("visibility")]
    public ActionResult<bool> GetVisibility([FromQuery] string suggestionId)
    {
        var suggestionGuid = Guid.Parse(suggestionId);
        var suggestion = _projectPageManager.GetSuggestion(suggestionGuid);
        return Ok(suggestion.IsGloballyVisible);
    }
    [HttpPost ("toggleExecuted")]
    public async Task<IActionResult> ToggleExecuted([FromQuery] string suggestionId)
    {
        var suggestionGuid = Guid.Parse(suggestionId);
        _projectPageManager.ChangeExecutedToggle(suggestionGuid);
        
        return Ok(new { success = true});
    }

    [HttpGet("executedValue")]
    public ActionResult<bool> GetExecutedValue([FromQuery] string suggestionId)
    {
        var suggestionGuid = Guid.Parse(suggestionId);
        var suggestion = _projectPageManager.GetSuggestion(suggestionGuid);
        return Ok(suggestion.IsExecuted);
    }
}