using BL.Interfaces;
using DAL.Interfaces;
using Domain.Admin;
using Domain.GlobalDtos;
using Domain.Interfaces;
using Domain.Interfaces.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.ViewModels;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;

namespace UI_MVC.Controllers;

[Authorize(Roles = "Admin")]
public class QuestionController : Controller
{
    private readonly IQuestionManager _questionManager;
    private readonly IQuestionWeightTipManager _questionWeightTipManager;
    private readonly ILogger<QuestionController> _logger;

    public QuestionController(IQuestionManager questionManager, ILogger<QuestionController> logger,
        IQuestionWeightTipManager questionWeightTipManager)
    {
        _questionManager = questionManager;
        _logger = logger;
        _questionWeightTipManager = questionWeightTipManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var questionObject = _questionManager.GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();

        var questionViewModels = questionObject.Select(q => new QuestionsViewModel()
        {
            Id = q.Id,
            Question = q.QuestionText,
            AnswerOptions = q.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText,
                AnswerOptionImpacts = ao.Impacts.Select(aoi => new AnswerOptionImpactDto()
                {
                    ParticipationMethodName = aoi.ParticipationMethod.Name,
                    ContributingWeight = aoi.ImpactWeight
                }).ToList()
            }).ToList()
        }).ToList();

        var allQuestionWeightTips = _questionWeightTipManager.GetAllQuestionWeightTips();
        var allWeightTipViewModels = allQuestionWeightTips.Select(t => new QuestionWeightTipsViewModel()
        {
            Id = t.Id,
            MinScore = t.MinScore,
            MaxScore = t.MaxScore,
            Message = t.Message,
        }).ToList();

        var allParticipationMethods = _questionWeightTipManager.GetAllParticipationMethods()
            .Select(p => new ParticipationViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });

        var model = new QuestionIndexViewModel()
        {
            Questions = questionViewModels,
            QuestionToEdit = new QuestionsViewModel(),
            QuestionWeightTips = allWeightTipViewModels,
            QuestionWeightTipViewModelToEdit = new QuestionWeightTipsViewModel(),
            ParticipationMethods = allParticipationMethods
        };

        return View(model);
    }
    
    [HttpPost]
    public IActionResult AddParticipationMethod(ParticipationViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Controleer de ingevoerde gegevens voor de score-tip. Er zijn fouten opgetreden.");
            return View("Index");
        }

        _questionWeightTipManager.AddParticipationMethod(viewModel.Name, viewModel.Description);
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult Delete(int id)
    {
        _questionManager.RemoveQuestion(id);
        _logger.LogInformation("Vraag succesvol verwijderd!");
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteParticipationMethod(Guid id)
    {
        _questionManager.RemoveParticipationMethod(id);
        _logger.LogInformation("Participationmethod succesvol verwijderd!");
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult DeleteQuestionWeightTip(int id)
    {
        try
        {
            _questionWeightTipManager.RemoveQuestionWeightTip(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fout bij verwijderen van score-tip met ID {id}.");
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetQuestionData(int id)
    {
        var domainQuestion = _questionManager.GetQuestionById(id);
        if (domainQuestion == null)
        {
            return NotFound();
        }

        var questionViewModel = new QuestionsViewModel
        {
            Id = domainQuestion.Id,
            Question = domainQuestion.QuestionText,
            AnswerOptions = domainQuestion.AnswerOptions.Select(ao => new AnswerOptionCrudViewModel
            {
                Id = ao.Id,
                AnswerOptionText = ao.AnswerOptionText
            }).ToList()
        };

        return Json(questionViewModel);
    }

    public IActionResult AddQuestion(QuestionViewModel viewModel)
    {
        var answers = viewModel.AnswerOptions;
        var question = _questionManager.AddQuestion(viewModel.QuestionText);
        foreach (var option in viewModel.AnswerOptions)
        {
            var mappedImpacts = option.Impacts.Select(i => new AnswerOptionImpactsDto
            {
                ParticipationMethodName = i.ParticipationMethodName,
                ImpactWeight = i.Impactweight
            }).ToList();
            _questionManager.AddAnswerOptionsWithImpacts(question.Id, option.AnswerText, mappedImpacts);
        }

        _logger.Log(LogLevel.Information, "viewModelParsed");
        return Ok();
    }
}