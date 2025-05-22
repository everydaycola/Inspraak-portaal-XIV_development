using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;

namespace UI_MVC.Controllers;

public class ExploreConceptController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IQuestionManager _questionManager;

    public ExploreConceptController(ILogger<HomeController> logger, IQuestionManager questionManager)
    {
        _logger = logger;
        _questionManager = questionManager;
    }

    public IActionResult Index()
    {
        var domainQuestions = _questionManager.GetAllQuestions();
        var questionsForView = domainQuestions.Select(q => new QuestionsViewModel
        {
            Id = q.Id,
            Question = q.QuestionText,
            Weight = q.Weight,
            AnswerOptions = q.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel()
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText, // Map domain property Text naar ViewModel AnswerOptionText
                Weight = ao.Weight
            }).ToList()
        }).ToList();

        var viewModel = new ExploreConceptViewModel
        {
            Questions = questionsForView,
            SubmittedAnswers =
                new List<QuestionAnswerViewModel>()
        };

        return View(viewModel);
    }
}