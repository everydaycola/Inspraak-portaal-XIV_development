using System.Diagnostics;
using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Models.ViewModels;
using IConfigurationManager = BL.Interfaces.IConfigurationManager;

namespace UI_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPanelManager _panelManager;
    private readonly IConfigurationManager _configurationManager;

    public HomeController(ILogger<HomeController> logger, IPanelManager panelManager,
        IConfigurationManager configurationManager)
    {
        _logger = logger;
        _panelManager = panelManager;
        _configurationManager = configurationManager;
    }

    public IActionResult Index()
    {
        var panels = _panelManager.GetAllPanelsWithPostsAndSuggestions();
        var data = _configurationManager.GetPlatformSettings();
        if (data != null)
        {
            return View(new HomePanelsViewModel
            {
                Panels = panels,
                DiscoverConceptText = data.DiscoverConceptText,
                AboutInspraakPortaalText = data.AboutInspraakPortaalText
            });
        }
        else
        {
            return View(new HomePanelsViewModel
            {
                Panels = panels
            });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Edit()
    {
        ViewBag.IsEditing = true;
        return View("Index");
    }
}