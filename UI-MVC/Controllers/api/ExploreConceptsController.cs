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
            var question = _questionManager.GetQuestionById(submittedAnswer.QuestionId);

            if (question == null)
            {
                return BadRequest($"Unknown question ID submitted: {submittedAnswer.QuestionId}");
            }

            var selectedOption = question.AnswerOptions
                .FirstOrDefault(ao => ao.Id == submittedAnswer.SelectedAnswerOptionId);

            if (selectedOption == null)
            {
                return BadRequest(
                    $"Unknown answer option ID '{submittedAnswer.SelectedAnswerOptionId}' for question ID '{submittedAnswer.QuestionId}'");
            }

            var score = selectedOption.Weight;
            totalScore += score;

            results.Add(new
            {
                QuestionId = submittedAnswer.QuestionId,
                QuestionText = question.QuestionText,
                SelectedAnswerText = selectedOption.AnswerOptionText,
                Score = score
            });
        }

        string suitabilityMessage = "Geen geschikte boodschap gevonden.";
        var allTips = _questionWeightTipManager.GetAllQuestionWeightTips();

        var applicableTip = allTips
            .OrderByDescending(t => t.MinScore)
            .FirstOrDefault(t => totalScore >= t.MinScore && totalScore <= t.MaxScore);


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
            TotalScore = totalScore,
            Suitability = suitabilityMessage,
            DetailedResults = results
        });
    }
}