using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class OrganisationController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOrganisationManager _organisationManager;

    public OrganisationController(ILogger<HomeController> logger, IOrganisationManager organisationManager)
    {
        _logger = logger;
        _organisationManager = organisationManager;
    }
    [HttpGet]
    public IActionResult OrganisationCreation()
    {
        return View();   
    }
    [HttpPost]
    public IActionResult OrganisationCreation(string name)
    {
        if (_organisationManager.GetOrganisationById(name) != null)
        {
            ModelState.AddModelError("", "Deze organisatie bestaat al, kies een andere naam!");
        }

        var organisation = _organisationManager.AddOrganisation(name, name, "#FFFFFF", "");
        return View();   
    }
}