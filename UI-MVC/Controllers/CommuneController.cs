using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers;

public class CommuneController : Controller
{
    private readonly ICommuneManager _communeManager;

    public CommuneController(ICommuneManager communeManager)
    {
        _communeManager = communeManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _communeManager.GetCommunes();
        return View(model);
    }
}