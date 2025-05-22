using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;

namespace UI_MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class ExploreConceptsController : ControllerBase
{
    private readonly IQuestionManager _questionManager; // Inject the manager

    public ExploreConceptsController(IQuestionManager questionManager)
    {
        _questionManager = questionManager;
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
                // This indicates a submitted SelectedAnswerOptionId that doesn't exist for this question
                return BadRequest($"Unknown answer option ID '{submittedAnswer.SelectedAnswerOptionId}' for question ID '{submittedAnswer.QuestionId}'");
            }

            // 3. Use the weight from the selected answer option
            var score = selectedOption.Weight;
            totalScore += score;

            results.Add(new
            {
                QuestionId = submittedAnswer.QuestionId, // Use QuestionId
                QuestionText = question.QuestionText,
                SelectedAnswerText = selectedOption.AnswerOptionText, // Use selectedOption.Text
                Score = score
            });
        }

        string suitabilityMessage;
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