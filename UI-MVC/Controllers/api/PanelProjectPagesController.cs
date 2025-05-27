using BL.Interfaces;
using Domain.Enums;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Posts.PostItems;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet("votePercentage")]
    public ActionResult<double> GetVotePercentage([FromQuery] string suggestionId, string postId)
    {
        if (!Guid.TryParse(suggestionId, out var suggestionGuid) || !Guid.TryParse(postId, out var postGuid))
        {
            return BadRequest("Invalid GUID format.");
        }

        var post = _projectPageManager.GetSuggestionPostSuggestionsAndWithVotes(postGuid);
        if (post == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        var suggestion = _projectPageManager.GetSuggestion(suggestionGuid);
        if (suggestion == null)
        {
            return NotFound($"Suggestion with ID {suggestionId} not found.");
        }

        return Ok(CalculateVotePercentage(post, suggestion));
    }

    private double CalculateVotePercentage(SuggestionPost post, Suggestion suggestion)
    {
        var totalvotes = post.Suggestions?.Sum(s => s.Votes?.Count ?? 0) ?? 0;
        var upvotes = suggestion.Votes?.Count(v => v.VoteType == VoteType.Up) ?? 0;
        var downvotes = suggestion.Votes?.Count(v => v.VoteType == VoteType.Down) ?? 0;
        if (totalvotes == 0 || downvotes > upvotes )
        {
            return 0;
        }

        return ((double)(upvotes - downvotes) / totalvotes) * 100;
    }
    
    [HttpPost("toggleVoting")]
    public async Task<IActionResult> EndSuggestionVoting([FromQuery] string postId)
    {
        if (!Guid.TryParse(postId, out var postGuid))
        {
            return BadRequest("Invalid GUID format.");
        }
        
        _projectPageManager.ChangeSuggestionPostVotingStatus(postGuid);
        return Ok(new { success = true });
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