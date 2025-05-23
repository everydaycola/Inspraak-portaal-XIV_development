using BL.Interfaces;
using Domain.Enums;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Posts.PostItems;
using Microsoft.AspNetCore.Mvc;

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
        var suggestionGuid = Guid.Parse(suggestionId);
        var postGuid = Guid.Parse(postId);

        var post =  (SuggestionPost)_projectPageManager.GetPost(postGuid);
        var suggestion = _projectPageManager.GetSuggestion(suggestionGuid);

        return CalculateVotePercentage(post, suggestion);
    }

    private double CalculateVotePercentage(SuggestionPost post, Suggestion suggestion)
    {
        var totalvotes = 0.0;
        foreach (var s in post.Suggestions)
        {
            totalvotes += s.Votes.Count;
        }

        var upvotes = 0;
        var downvotes = 0;
        foreach (var vote in suggestion.Votes)
        {
            if (vote.VoteType.Equals(VoteType.Up))
            {
                upvotes++;
            }
            else if (vote.VoteType.Equals(VoteType.Down))
            {
                downvotes++;
            }
        }

        return (upvotes - downvotes) / totalvotes;
    }
}