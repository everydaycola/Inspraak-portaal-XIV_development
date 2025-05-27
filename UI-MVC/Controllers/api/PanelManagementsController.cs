using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class PanelManagementsController : ControllerBase
{

    private readonly IPanelManager _manager;

    public PanelManagementsController(IPanelManager manager)
    {
        _manager = manager;
    }

    [HttpPost("UpdatePlanningsGroupMember")]
    public async Task<IActionResult> UpdatePlanningsGroupMember([FromBody] UpdatePlanningGroupMemberViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _manager.UpdatePlanningsGroupMember(
                viewModel.UserId,
                viewModel.Email,
                viewModel.Naam,
                viewModel.Functie
            );

            return Ok(new { success = true, message = "Lid succesvol bijgewerkt." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij het bijwerken van lid: {ex.Message}");
            return StatusCode(500, new { success = false, message = $"Er is een fout opgetreden: {ex.Message}" });
        }
    }
}