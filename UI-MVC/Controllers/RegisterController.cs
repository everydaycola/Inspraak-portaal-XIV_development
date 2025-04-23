using System.Collections;
using System.Security.Permissions;
using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;
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
        var member = _manager.GetPanelMemberWithPanel(userId);
        var panel = _manager.GetPanelWithCriteriaAndCriteriaAnswerOptions(member.Panel.Id);
        var critResponse = member.Responses;
        IEnumerable<CriteriaResponse> defaultCriteria = critResponse.Where(c => c.Criteria.IsDefault);
        IEnumerable<Criteria> nonDefaultCriteriaWithoutResponse = panel.Criteria
            .Where(c => !c.IsDefault);
        
        return View(new NewPanelMemberDto
        {
            PanelId = member.Panel.Id.ToString(),
            UserId = userId.ToString(),
            Email = member.Email,
            IsRegistrationOpen = member.Panel.IsRegistrationOpen,
            HasAnsweredQuestions = member.HasRegistered,
            NonDefaultCriteria = nonDefaultCriteriaWithoutResponse,
            DefaultCriteria= defaultCriteria
        });
    }
    
    [HttpPost]
    public IActionResult SubmitExtraQuestionForm(ExtraQuestionFormAnswersDto formData)
    {
        if (ModelState.IsValid)
        {
            var email = formData.Email;
            PanelMember member = _manager.GetPanelMemberWithCriteriaResponses(formData.UserId);
            member.Email = email;
            member.HasRegistered = true;
            _manager.UpdatePanelRegistrationCount(formData.PanelId, true);
            _critManager.SavePanelMemberCriteriaResponses(formData.PanelId, formData.CriteriaAnswers,member);
            PanelMember updatedMember = _manager.UpdatePanelMember(member);
            
            return View("Index", new NewPanelMemberDto
            {
                PanelId = updatedMember.Panel.Id.ToString(),
                UserId = updatedMember.PanelMemberId.ToString(),
                HasAnsweredQuestions = updatedMember.HasRegistered,
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

    [HttpPost]
    public IActionResult RegisterRandomUsers(Guid guid, int count)
    {
        // mostly a testing function to add random users to your panel
        // way too much logic for a controller, but ok since it's a dev feature
        
        var panel = _manager.GetPanelWithCriteriaAndCriteriaAnswerOptions(guid);
        var members = _manager.GetAllPanelMembersForPanel(guid)
            .Where(pm => !pm.HasRegistered)
            .Take(count); // Limit to the first `count` members
        
        var random = new Random();
        
        foreach (var member in members)
        {
            var responses = new Dictionary<string, string>();
        
            foreach (var criterion in panel.Criteria.Where(c => !c.IsDefault))
            {
                var randomValue = random.NextDouble();
                var accumulatedWeight = 0.0;
    
                // Find the item whose accumulated weight range contains the random value
                foreach (var option in criterion.AnswerOptions)
                {
                    accumulatedWeight += option.DistributionPercentage;
                    if (randomValue > accumulatedWeight) continue;
                    responses.Add(criterion.Name, option.Option);
                    break;
                }
            }
        
            _critManager.SavePanelMemberCriteriaResponses(panel.Id, responses, member);
            _manager.UpdatePanelRegistrationCount(panel.Id, true);
            member.HasRegistered = true;
            member.Email = "placeholder@gmail.com";
            _manager.UpdatePanelMember(member);
        }
        
        return RedirectToAction("Index", "PanelManagement",new { id = guid });
    }
}