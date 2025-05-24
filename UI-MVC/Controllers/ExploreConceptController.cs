using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
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
        var domainQuestions = _questionManager.GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
        var questionsForView = domainQuestions.Select(q => new QuestionsViewModel
        {
            Id = q.Id,
            Question = q.QuestionText,
            AnswerOptions = q.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel()
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText,
                Weight = ao.Weight,
                AnswerOptionImpacts = ao.Impacts.Select(aoi => new AnswerOptionImpactDto()
                {
                    ParticipationMethodName = aoi.ParticipationMethod.Name,
                    ContributingWeight = aoi.ImpactWeight
                }).ToList()
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