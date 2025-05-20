using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class OrganisationController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOrganisationManager _organisationManager;

    [HttpPost]
    public IActionResult OrganisationCreation(string name)
    {
        if (_organisationManager.GetOrganisationById(name) != null)
        {
            ModelState.AddModelError("", "Deze organisatie bestaat al, kies een andere naam!");
        }
        var organisation = _organisationManager.AddOrganisation(name.ToLower(), name, "#FFFFFF", "");
        
        var subdomain = organisation.Name.ToLowerInvariant();
        var currentDomain = HttpContext.Request.Host.Host;
        var protocol = HttpContext.Request.Scheme;

        if (currentDomain.StartsWith("www."))
        {
            currentDomain = currentDomain.Substring(4);
        }
        
        var baseDomain = currentDomain.Split('.').Skip(currentDomain.Split('.').Length - 2).Aggregate((a, b) => a + "." + b);
        var newUrl = $"{protocol}://{subdomain}.{baseDomain}/"; // Or /{controller}/Details if using controller

        return Redirect(newUrl);   
    }

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
}