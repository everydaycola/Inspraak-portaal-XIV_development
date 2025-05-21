using BL.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionManager _questionManager;

    public QuestionController(IQuestionManager questionManager)
    {
        _questionManager = questionManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var questions = _questionManager.GetAllQuestions()
            .Select(q => new QuestionManagementViewModel()
            {
                Id = q.Id,
                Question = q.QuestionText,
                // Voeg andere relevante eigenschappen toe
            })
            .ToList();

        return View(questions);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(QuestionManagementViewModel model)
    {
        if (ModelState.IsValid)
        {
            var answerOptions = model.AnswerOptions?
                .Select(optionText => new AnswerOption { AnswerOptionText = optionText.AnswerOptionText })
                .ToList() as ICollection<AnswerOption>;

            var newQuestion = _questionManager.AddQuestion(model.Id, model.Question, answerOptions);
            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var question = _questionManager.GetQuestionById(id);
        if (question == null)
        {
            return NotFound();
        }

        var model = new QuestionManagementViewModel()
        {
            Id = question.Id,
            Question = question.QuestionText,
            AnswerOptions = question.AnswerOptions?
                .Select(optionText => new AnswerOption { AnswerOptionText = optionText.AnswerOptionText })
                .ToList()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(QuestionManagementViewModel model)
    {
        if (ModelState.IsValid)
        {
            _questionManager.UpdateQuestion(model.Id, model.Question,
                model.AnswerOptions /* andere properties */);
            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _questionManager.RemoveQuestion(id);
        return RedirectToAction("Index");
    }
}