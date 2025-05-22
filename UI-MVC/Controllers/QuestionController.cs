using BL.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;

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
    public IActionResult AddOrUpdate(QuestionIndexViewModel fullViewModel)
    {
        QuestionManagementViewModel questionToManage = fullViewModel.QuestionToEdit;
        questionToManage.AnswerOptions ??= new List<AnswerOptionCrudViewModel>();
        var allDomainQuestions = _questionManager.GetAllQuestions();

        fullViewModel.Questions = allDomainQuestions.Select(q => new QuestionManagementViewModel
        {
            Id = q.Id,
            Question = q.QuestionText,
            AnswerOptions = q.AnswerOptions?.Select(ao => new AnswerOptionCrudViewModel
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText,
                Weight = ao.Weight
            }).ToList()
        }).ToList();

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Controleer de ingevoerde gegevens. Er zijn fouten opgetreden.");
            return View("Index", fullViewModel);
        }

        try
        {
            List<AnswerOption> domainAnswerOptions = new List<AnswerOption>();
            foreach (var crudOption in questionToManage.AnswerOptions)
            {
                domainAnswerOptions.Add(new AnswerOption
                {
                    Id = crudOption.Id,
                    AnswerOptionText = crudOption.AnswerOptionText,
                    Weight = crudOption.Weight
                });
            }

            if (questionToManage.Id == 0) // New question
            {
                var newDomainQuestion = new Question
                {
                    QuestionText = questionToManage.Question,
                    AnswerOptions = domainAnswerOptions
                };
                _questionManager.AddQuestion(newDomainQuestion.Id, questionToManage.Question,
                    newDomainQuestion.AnswerOptions);
            }
            else
            {
                var existingDomainQuestion = _questionManager.GetQuestionById(questionToManage.Id);
                if (existingDomainQuestion == null)
                {
                    throw new InvalidOperationException(
                        $"Question with ID {questionToManage.Id} not found for update.");
                }

                existingDomainQuestion.QuestionText = questionToManage.Question;

                existingDomainQuestion.AnswerOptions.Clear();
                foreach (var ao in domainAnswerOptions)
                {
                    existingDomainQuestion.AnswerOptions.Add(ao);
                }

                _questionManager.UpdateQuestion(existingDomainQuestion.Id, existingDomainQuestion.QuestionText,
                    existingDomainQuestion.AnswerOptions);
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij opslaan van vraag.");
            return View("Index", fullViewModel);
        }
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