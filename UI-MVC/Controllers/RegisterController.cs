using System.Collections;
using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.Dto.Register;

namespace UI_MVC.Controllers;

public class RegisterController : Controller
{
    private readonly ILogger<RegisterController> _logger;
    private readonly IPanelManager _manager;
    private readonly ICriteriaManager _critManager;

    public RegisterController(ILogger<RegisterController> logger,IPanelManager manager, ICriteriaManager critManager)
    {
        _logger = logger;
        _manager = manager;
        _critManager = critManager;
    }

    [HttpGet]
    public IActionResult Index(Guid userId)
    {
        PanelMember member = _manager.GetPanelMemberWithPanel(userId);
        IEnumerable<Criteria> criteria = _critManager.GetAllDefaultCriteriaWithValuesForPanel(member.Panel.Id);
        
        return View(new NewPanelMemberDto
        {
            PanelId = member.Panel.Id.ToString(),
            UserId = userId.ToString(),
            Email = member.Email,
            IsRegistrationOpen = member.Panel.IsRegistrationOpen,
            HasAnsweredQuestions = member.hasAnsweredAllQuestions,
            criteria = criteria
        });
    }
    
    [HttpGet]
    public IActionResult NewUserTemp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitExtraQuestionForm(ExtraQuestionFormAnswersDto formData)
    {
        if (ModelState.IsValid)
        {
            foreach (var answer in formData.CriteriaAnswers)
            {
                var criterium = answer.Key;
                var userAnswer = answer.Value;
                // Verwerken, opslaan etc.
            }
            
            var email = formData.Email;
            PanelMember member = _manager.GetPanelMemberById(formData.UserId);
            member.Email = email;
            member.hasAnsweredAllQuestions = true;
            _manager.UpdatePanelRegistrationCount(formData.PanelId, true);
            PanelMember updatedMember = _manager.UpdatePanelMember(member);
            
            return View("Index", new NewPanelMemberDto
            {
                PanelId = updatedMember.Panel.Id.ToString(),
                UserId = updatedMember.PanelMemberId.ToString(),
                HasAnsweredQuestions = updatedMember.hasAnsweredAllQuestions,
                Email = updatedMember.Email
            });
        }
        
        ModelState.AddModelError("", "Invalid form data.");
        return View("Index", new NewPanelMemberDto
        {
            PanelId = formData.PanelId.ToString(),
            UserId = formData.UserId.ToString(),
            HasAnsweredQuestions = false
        });
    }
}