using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;

namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class ExploreConceptsController : ControllerBase
{
    private readonly IQuestionManager _questionManager; // Inject the manager
    private readonly IQuestionWeightTipManager _questionWeightTipManager;

    public ExploreConceptsController(IQuestionManager questionManager,
        IQuestionWeightTipManager questionWeightTipManager)
    {
        _questionManager = questionManager;
        _questionWeightTipManager = questionWeightTipManager;
    }

    [HttpGet("SubmitAnswers")]
    public IActionResult SubmitAnswers([FromQuery]int totalWeight)
    {
        string suitabilityMessage = "Geen geschikte boodschap gevonden.";
        var allTips = _questionWeightTipManager.GetAllQuestionWeightTips();

        var applicableTip = allTips
            .OrderByDescending(t => t.MinScore)
            .FirstOrDefault(t => totalWeight >= t.MinScore && totalWeight <= t.MaxScore);
        if (applicableTip != null)
        {
            suitabilityMessage = applicableTip.Message;
        }
        else
        {
            suitabilityMessage = "Score is te laag om een passende tip te geven.";
        }
        return Ok(new
        {
            Suitability = suitabilityMessage
        });
    }
}