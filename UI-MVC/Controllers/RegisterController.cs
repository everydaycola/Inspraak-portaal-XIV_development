using BL;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class RegisterController : Controller
{
    private readonly IManager _manager;

    public RegisterController(IManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult Index(string userId, string panelId)
    {
        Console.WriteLine($"userId: {userId}, panelId: {panelId}");
        return View(new NewPanelMemberDto
        {
            PanelId = panelId,
            UserId = userId
        });
    }
    
    [HttpGet]
    public IActionResult NewUserTemp()
    {
        return View();
    }
}