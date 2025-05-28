using BL.Interfaces;
using Domain.GlobalDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.ViewModels;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels.management;
using AnswerOptionImpactsDto = Domain.GlobalDtos.AnswerOptionImpactsDto;


namespace UI_MVC.Controllers;

[Authorize(Roles = "Admin")]
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
        var questionObject = _questionManager.GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
        var questionViewModels = questionObject.Select(q => new QuestionsViewModel()
        {
            Id = q.Id,
            Question = q.QuestionText,
            AnswerOptions = q.AnswerOptions.Select(ao => new AnswerOptionDto()
            {
                Id = ao.Id,
                AnswerText = ao.AnswerOptionText,
                Impacts = ao.Impacts.Select(aoi => new AnswerOptionImpactsDto()
                {
                    ParticipationMethodName = aoi.ParticipationMethod.Name,
                    ImpactWeight = aoi.ImpactWeight
                }).ToList()
            }).ToList()
        }).ToList();


        var allParticipationMethods = _questionManager.GetAllParticipationMethods()
            .Select(p => new ParticipationViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });
        var model = new QuestionIndexViewModel()
        {
            Questions = questionViewModels,
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

        _questionManager.AddParticipationMethod(viewModel.Name, viewModel.Description);
        return RedirectToAction("Index");
    }

    public IActionResult AddQuestion(QuestionViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", viewModel);
        }

        var question = _questionManager.AddQuestion(viewModel.QuestionText);
        foreach (var option in viewModel.AnswerOptions)
        {
            var mappedImpacts = option.Impacts.Select(i => new AnswerOptionImpactsDto
            {
                ParticipationMethodName = i.ParticipationMethodName,
                ImpactWeight = i.ImpactWeight
            }).ToList();
            _questionManager.AddAnswerOptionsWithImpacts(question.Id, option.AnswerText, mappedImpacts);
        }

        _logger.Log(LogLevel.Information, "Succesfully added a new question.");
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult DeleteQuestionWithAnswerOptions(int questionId)
    {
        _questionManager.DeleteQuestionWithAnswerOptionsAndImpacts(questionId);
        _logger.LogInformation("Vraag met id " + questionId + " en zijn antwoord opties werden succesvol verwijderd. ");
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteParticipationMethod(Guid id)
    {
        _questionManager.DeleteParticipationMethod(id);
        _logger.LogInformation("Participationmethod succesvol verwijderd!");
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditParticipationMethod(Guid id)
    {
        var method = _questionManager.GetParticipationMethodById(id);
        if (method == null)
            return NotFound();

        var viewModel = new ParticipationViewModel
        {
            Id = method.Id,
            Name = method.Name,
            Description = method.Description
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult EditParticipationMethod(ParticipationViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        _questionManager.UpdateParticipationMethod(viewModel.Id, viewModel.Name, viewModel.Description);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditQuestion(int id)
    {
        var question = _questionManager.GetQuestionWithAnswerOptionsAndImpactsAndParticipationMethod(id);
        if (question == null) return NotFound();

        var allParticipationMethods = _questionManager.GetAllParticipationMethods()
            .Select(pm => new ParticipationViewModel
            {
                Id = pm.Id,
                Name = pm.Name,
                Description = pm.Description
            }).ToList();

        var viewModel = new QuestionViewModel
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            AnswerOptions = question.AnswerOptions.Select(ao => new AnswerOptionDto
            {
                Id = ao.Id,
                AnswerText = ao.AnswerOptionText,
                Impacts = ao.Impacts.Select(i => new AnswerOptionImpactsDto()
                {
                    ParticipationMethodId = i.ParticipationMethod.Id,
                    ParticipationMethodName = i.ParticipationMethod.Name,
                    ImpactWeight = i.ImpactWeight
                }).ToList()
            }).ToList(),
            ParticipationMethods = allParticipationMethods
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult EditQuestion(QuestionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ParticipationMethods = _questionManager.GetAllParticipationMethods()
                .Select(pm => new ParticipationViewModel
                {
                    Id = pm.Id,
                    Name = pm.Name,
                    Description = pm.Description
                }).ToList();
            return View(model);
        }

        _questionManager.UpdateQuestionWithAnswerOptions(
            model.Id,
            model.QuestionText,
            model.AnswerOptions.Select(ao => new AnswerOptionDto
            {
                Id = ao.Id,
                AnswerText = ao.AnswerText,
                Impacts = ao.Impacts.Select(i => new AnswerOptionImpactsDto
                {
                    ParticipationMethodId = i.ParticipationMethodId,
                    ParticipationMethodName = i.ParticipationMethodName,
                    ImpactWeight = i.ImpactWeight
                }).ToList()
            }).ToList()
        );

        return RedirectToAction("Index");
    }
}