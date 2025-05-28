using System.Diagnostics;
using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPanelManager _panelManager;

    public HomeController(ILogger<HomeController> logger, IPanelManager panelManager)
    {
        _logger = logger;
        _panelManager = panelManager;
    }

    public IActionResult Index()
    {
        var panels = _panelManager.GetAllPanelsWithPostsAndSuggestions();
        
        return View(new HomePanelsViewModel
        {
            Panels = panels
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}