using BL.Interfaces;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto.PostDtos;


namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class VotesController : ControllerBase
{
    private readonly IPanelProjectPageManager _manager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<VotesController> _logger;

    public VotesController(IPanelProjectPageManager manager, UserManager<ApplicationUser> userManager, ILogger<VotesController> logger)
    {
        _manager = manager;
        _userManager = userManager;
        _logger = logger;
    }
    
    [HttpPut]
    public async Task<IActionResult> AddNewVote([FromBody] NewVoteDto newVote)
    {
        var user = await _userManager.GetUserAsync(User);
        
        if (user == null)
        {
            return Unauthorized(); // User not found or not authenticated
        }

        
        if (
            !Guid.TryParse(newVote.SuggestionId, out var suggestionGuid) ||
            !Enum.TryParse<VoteType>(newVote.Type, out var voteType))
        {
            return NotFound();
        }

        _manager.ChangeVote(
            user,
            suggestionGuid,
            voteType);
        
        return NoContent();
    }
}