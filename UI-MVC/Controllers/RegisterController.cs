using System.Collections;
using System.Security.Permissions;
using BL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI_MVC.Models;
using UI_MVC.Models.Dto;
using UI_MVC.Models.Dto.Register;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;

public class RegisterController : Controller
{
    private readonly ILogger<RegisterController> _logger;
    private readonly IPanelManager _manager;
    private readonly ICriteriaManager _critManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterController(ILogger<RegisterController> logger,IPanelManager manager, ICriteriaManager critManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _manager = manager;
        _critManager = critManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    //This method returns the page with criteria which a user can answer.
    [HttpGet]
    public IActionResult Index(Guid userId)
    {
        var member = _manager.GetPanelMemberWithPanel(userId);
        var panel = _manager.GetPanelWithCriteriaAndCriteriaAnswerOptions(member.Panel.Id);
        var critResponse = member.Responses;
        IEnumerable<CriteriaResponse> defaultCriteria = critResponse.Where(c => c.Criteria.IsDefault);
        IEnumerable<Criteria> nonDefaultCriteriaWithoutResponse = panel.Criteria
            .Where(c => !c.IsDefault);
        
        return View(new NewPanelMemberViewModel
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
    //The user has answered the extra questions succesfully.
    [HttpPost]
    public IActionResult SubmitExtraQuestionForm(ExtraQuestionFormAnswersDto formData)
    {
        if (ModelState.IsValid)
        {
            var member = _manager.GetPanelMemberWithCriteriaResponses(formData.UserId);
            member.Email = formData.Email;
            member.HasRegistered = true;
            _manager.UpdatePanelRegistrationCount(formData.PanelId, true);
            _critManager.SavePanelMemberCriteriaResponses(formData.PanelId, formData.CriteriaAnswers,member);
            var updatedMember = _manager.UpdatePanelMember(member);
            
            return View("Index", new NewPanelMemberViewModel
            {
                PanelId = updatedMember.Panel.Id.ToString(),
                UserId = updatedMember.PanelMemberId.ToString(),
                HasAnsweredQuestions = updatedMember.HasRegistered,
                Email = updatedMember.Email
            });
        }
        
        ModelState.AddModelError("", "Invalid form data.");
        return View("Index", new NewPanelMemberViewModel
        {
            PanelId = formData.PanelId.ToString(),
            UserId = formData.UserId.ToString(),
            HasAnsweredQuestions = false
        });
    }
    
    // mostly a testing function to add random users to your panel
    // way too much logic for a controller, but ok since it's a dev feature
    [HttpPost]
    public IActionResult RegisterRandomUsers(Guid guid, int count)
    {
        
        
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
                var selectedOption = false;
    
                // Find the item whose accumulated weight range contains the random value
                foreach (var option in criterion.AnswerOptions)
                {
                    accumulatedWeight += option.DistributionPercentage;
                    if (randomValue > accumulatedWeight) continue;
                    responses.Add(criterion.Name, option.Option);
                    selectedOption = true;
                    break;
                }
                
                
                // when all distributions are 0 meaning dist is unknown
                if (accumulatedWeight == 0)
                {
                    var answerOptionsList = criterion.AnswerOptions.ToList();
                    responses.Add(criterion.Name, answerOptionsList[random.Next(answerOptionsList.Count)].Option);
                    selectedOption = true;
                }
                
                // In case, due to rounding errors, nothing is selected, select the last one
                if (!selectedOption) responses.Add(criterion.Name, criterion.AnswerOptions.Last().Option);
            }
        
            _critManager.SavePanelMemberCriteriaResponses(panel.Id, responses, member);
            _manager.UpdatePanelRegistrationCount(panel.Id, true);
            member.HasRegistered = true;
            member.Email = "placeholder@gmail.com";
            _manager.UpdatePanelMember(member);
        }
        
        return RedirectToAction("Index", "PanelManagement",new { id = guid });
    }

    [HttpGet]
    public IActionResult AccountCreation(Guid userId)
    {
        var panelMember = _manager.GetPanelMemberWithPanel(userId);
        var model = new RegisterViewModel
        {
            UserUniqueCode = userId,
            Email = panelMember.Email
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AccountCreation(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var panelMember = _manager.GetPanelMemberWithPanel(model.UserUniqueCode);
        panelMember.User = new ApplicationUser()
        {
            UserName = model.Email,
            NormalizedUserName = model.Email.ToUpper(),
            Email = model.Email,
            NormalizedEmail = model.Email.ToUpper()
        };
        var user = panelMember.User;
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, CustomIdentityConstants.PanelMemberRole);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }
}