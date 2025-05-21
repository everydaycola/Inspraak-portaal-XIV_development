using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class ExploreConceptsController : ControllerBase
{
    private static readonly Dictionary<int, int> _questionWeights = new Dictionary<int, int>
    {
        { 0, 5 },
        { 1, 10 },
        { 2, 7 }
    };

    [HttpPost("SubmitAnswers")]
    public IActionResult SubmitAnswers([FromForm] ExploreConceptViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var totalScore = 0;
        var results = new List<object>();
        foreach (var submittedAnswer in model.SubmittedAnswers)
        {
            if (_questionWeights.TryGetValue(submittedAnswer.Id, out var weight))
            {
                if (submittedAnswer.Answer)
                {
                    totalScore += weight;
                    results.Add(new { QuestionId = submittedAnswer.Id, Answer = "Ja", Score = weight });
                }
                else
                {
                    results.Add(new { QuestionId = submittedAnswer.Id, Answer = "Nee", Score = 0 });
                }
            }
            else
            {
                return BadRequest($"Unknown question ID submitted: {submittedAnswer.Id}");
            }
        }

        string suitabilityMessage = "Not yet determined.";
        if (totalScore >= 20)
        {
            suitabilityMessage = "Een burgerpanel is zeer geschikt voor uw organisatie!";
        }
        else if (totalScore >= 10)
        {
            suitabilityMessage = "Een burgerpanel kan nuttig zijn, maar vereist mogelijk aanvullende overwegingen.";
        }
        else
        {
            suitabilityMessage = "Een burgerpanel is mogelijk niet de meest geschikte aanpak voor uw huidige situatie.";
        }


        return Ok(new
        {
            TotalScore = totalScore,
            Suitability = suitabilityMessage,
            DetailedResults = results
        });
    }
}