using BL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto.ProjectPage;

namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class VotesController : ControllerBase
{
    private readonly IPanelProjectPageManager _manager;
    private readonly ICustomUserManager _customUserManager;
    private readonly UserManager<ApplicationUser> _userManager; 

    public VotesController(IPanelProjectPageManager manager, ICustomUserManager customUserManager, UserManager<ApplicationUser> userManager)
    {
        _manager = manager;
        _customUserManager = customUserManager;
        _userManager = userManager;
    }
    
    [HttpPut]
    public IActionResult AddNewVote([FromBody] NewVoteDto newVote)
    {

        if (!Guid.TryParse(_userManager.GetUserId(User), out var userId))
        {
            return Forbid();
        }
        
        var vote = _manager.ChangeVote(newVote.SuggestionId, userId, newVote.Type)



        return NoContent();
    }
}