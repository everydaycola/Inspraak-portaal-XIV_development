using BL.Interfaces;
using DAL.Interfaces;
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
                Weight = ao.Weight,
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
    public IActionResult AddOrUpdate(QuestionIndexViewModel fullViewModel)
    {
        QuestionsViewModel questionToManage = fullViewModel.QuestionToEdit;
        questionToManage.AnswerOptions ??= new List<AnswerOptionCrudViewModel>();
        var allDomainQuestions = _questionManager.GetAllQuestions();

        fullViewModel.Questions = allDomainQuestions.Select(q => new QuestionsViewModel
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
    public IActionResult AddOrUpdateScoreRangeTip(QuestionIndexViewModel fullViewModel)
    {
        QuestionWeightTipsViewModel tipToManage = fullViewModel.QuestionWeightTipViewModelToEdit;


        var allDomainQuestions = _questionManager.GetAllQuestions();
        fullViewModel.Questions = allDomainQuestions.Select(q => new QuestionsViewModel
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

        var allDomainTips = _questionWeightTipManager.GetAllQuestionWeightTips();
        fullViewModel.QuestionWeightTips = allDomainTips.Select(t => new QuestionWeightTipsViewModel
        {
            Id = t.Id,
            MinScore = t.MinScore,
            MaxScore = t.MaxScore,
            Message = t.Message
        }).ToList();


        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Controleer de ingevoerde gegevens voor de score-tip. Er zijn fouten opgetreden.");
            return View("Index", fullViewModel);
        }

        try
        {
            var domainTip = new QuestionWeightTips()
            {
                Id = tipToManage.Id,
                MinScore = tipToManage.MinScore,
                MaxScore = tipToManage.MaxScore ?? int.MaxValue,
                Message = tipToManage.Message
            };

            if (domainTip.Id == 0)
            {
                _questionWeightTipManager.AddQuestionWeightTip(domainTip.Id, tipToManage.MinScore,
                    tipToManage.MaxScore ?? 0, tipToManage.Message);
            }
            else
            {
                _questionWeightTipManager.UpdateQuestionWeightTip(domainTip.Id, tipToManage.MinScore,
                    tipToManage.MaxScore ?? 0, tipToManage.Message);
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij opslaan van score-tip.");
            return View("Index", fullViewModel);
        }
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
                AnswerOptionText = ao.AnswerOptionText,
                Weight = ao.Weight
            }).ToList()
        };

        return Json(questionViewModel);
    }
}