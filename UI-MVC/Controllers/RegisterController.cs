using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.Dto.Register;

namespace UI_MVC.Controllers;

public class RegisterController : Controller
{
    private readonly IPanelManager _manager;

    public RegisterController(IPanelManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult Index(Guid userId)
    {
        PanelMember member = _manager.GetPanelMemberWithPanel(userId);
        
        Criteria crit1 = new Criteria("Rijbewijs");
        CriteriaValue val1 = new CriteriaValue("Ja", 0.5);
        CriteriaValue val2 = new CriteriaValue("Nee", 0.5);
        crit1.Values.Add(val1);
        crit1.Values.Add(val2);
        Criteria crit2 = new Criteria("Vervoersmiddel");
        CriteriaValue val3 = new CriteriaValue("Fiets", 1/3);
        CriteriaValue val4 = new CriteriaValue("Auto", 1/3);
        CriteriaValue val5 = new CriteriaValue("Te voet", 1/3);
        crit2.Values.Add(val3);
        crit2.Values.Add(val4);
        crit2.Values.Add(val5);
        
        return View(new NewPanelMemberDto
        {
            PanelId = member.Panel.Id.ToString(),
            UserId = userId.ToString(),
            Email = member.Email,
            criteriaList = new List<Criteria>()
            {
                crit1,
                crit2
            },
            HasAnsweredQuestions = false
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
            var email = formData.Email;
            PanelMember member = _manager.GetPanelMemberById(formData.UserId);
            member.Email = email;
            member.hasAnsweredAllQuestions = true;
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