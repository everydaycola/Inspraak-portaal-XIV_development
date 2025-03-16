using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

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
        var member = _manager.GetPanelByUserId(userId);
        Console.WriteLine($"userId: {userId}, panelId: {member.Panel.Id.ToString()}");
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
            criteriaList = new List<Criteria>()
            {
                crit1,
                crit2
            }
        });
    }
    
    [HttpGet]
    public IActionResult NewUserTemp()
    {
        return View();
    }
}