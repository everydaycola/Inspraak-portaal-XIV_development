using System.Diagnostics;
using BL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles=CustomIdentityConstants.AdminRole)]
    public IActionResult Edit()
    {
        var data = _configurationManager.GetPlatformSettings();
        if(data != null){
            return View("EditHomepage", new HomePageContentViewModel()
            {
                AboutInspraakPortaalText = data.AboutInspraakPortaalText,
                DiscoverConceptText = data.DiscoverConceptText
            });
        }

        return RedirectToAction("Error");
    }
    
    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.AdminRole)]
    //[ValidateAntiForgeryToken]
    public IActionResult Edit(HomePageContentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("EditHomepage", model);
        }
        _configurationManager.SavePlatformSettings(
            model.DiscoverConceptText,
            model.AboutInspraakPortaalText
        );
        return RedirectToAction("Index");
    }
    
}