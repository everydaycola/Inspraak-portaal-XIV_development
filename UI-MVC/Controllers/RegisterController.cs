using BL;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class RegisterController : Controller
{
    private readonly PanelManager _manager;

    public RegisterController(IManager manager)
    {
        _manager = (PanelManager)manager;
    }

    [HttpGet]
    public IActionResult Index(Guid userId)
    {
        var member = _manager.GetPanelByUserId(userId);
        Console.WriteLine($"userId: {userId}, panelId: {member.Panel.Id.ToString()}");
        return View(new NewPanelMemberDto
        {
            PanelId = member.Panel.Id.ToString(),
            UserId = userId.ToString()
        });
    }
    
    [HttpGet]
    public IActionResult NewUserTemp()
    {
        return View();
    }
}