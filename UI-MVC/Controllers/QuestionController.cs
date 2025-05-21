using BL.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionManager _questionManager;
    private readonly ILogger<QuestionController> _logger;

    public QuestionController(IQuestionManager questionManager, ILogger<QuestionController> logger)
    {
        _questionManager = questionManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var questionObject = _questionManager.GetAllQuestions();

        var questionViewModels = questionObject.Select(q => new QuestionManagementViewModel()
        {
            Id = q.Id,
            Question = q.QuestionText,
            AnswerOptions = q.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText,
                Weight = ao.Weight
            }).ToList()
        }).ToList();

        var model = new QuestionIndexViewModel()
        {
            Questions = questionViewModels,
            QuestionToEdit = new QuestionManagementViewModel()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult AddOrUpdate(QuestionManagementViewModel model)
    {
        // This is the problematic part you mentioned, now it should work better
        if (!ModelState.IsValid)
        {
            var allDomainQuestions = _questionManager.GetAllQuestions();

            var allQuestionViewModels = allDomainQuestions.Select(q => new QuestionManagementViewModel
            {
                Id = q.Id,
                Question = q.QuestionText,
                AnswerOptions = q.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel
                {
                    Id = ao.Id,
                    AnswerOptionText = ao.AnswerOptionText,
                    Weight = ao.Weight
                }).ToList()
            }).ToList();

            var indexModel = new QuestionIndexViewModel()
            {
                Questions = allQuestionViewModels,
                QuestionToEdit = model
            };

            _logger.LogInformation("Controleer de ingevoerde gegevens. Er zijn fouten opgetreden.");
            return View("Index", indexModel);
        }

        return View("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _questionManager.RemoveQuestion(id);
        _logger.LogInformation("Vraag succesvol verwijderd!");
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetQuestionData(int id)
    {
        // Manager returns Domain.Models.Question
        var domainQuestion = _questionManager.GetQuestionById(id);
        if (domainQuestion == null)
        {
            return NotFound();
        }

        // Map Domain.Models.Question to UI_MVC.Models.ViewModels.QuestionManagementViewModel for JavaScript
        var questionViewModel = new QuestionManagementViewModel
        {
            Id = domainQuestion.Id,
            Question = domainQuestion.QuestionText,
            AnswerOptions = domainQuestion.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText,
                Weight = ao.Weight
            }).ToList()
        };

        return Json(questionViewModel); // Return as JSON
    }
}