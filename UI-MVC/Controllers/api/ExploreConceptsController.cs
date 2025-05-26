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

    [HttpPost("SubmitAnswers")]
    public IActionResult SubmitAnswers([FromBody] Dictionary<string, int> totalWeightsByMethod)
    {
        var highestWeightEntry = totalWeightsByMethod.Aggregate((l, r) => l.Value > r.Value ? l : r);
        string highestParticipationMethodName = highestWeightEntry.Key;
        
        var applicableParticipationMethod = _questionManager.GetParticipationMethodByName(highestParticipationMethodName);

        var suitabilityMessage = "Score is te laag om een passende tip te geven.";
        if (applicableParticipationMethod != null)
        {
            suitabilityMessage = applicableParticipationMethod.Description;
        }
        return Ok(new
        {
            Suitability = suitabilityMessage
        });
    }
}