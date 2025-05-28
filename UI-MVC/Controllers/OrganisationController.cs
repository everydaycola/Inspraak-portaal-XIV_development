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
        name = name.Trim().Replace(" ", "-");
        
        const string subdomainPattern = @"^[a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, subdomainPattern))
        {
            ModelState.AddModelError("", "De organisatienaam mag alleen letters, cijfers en koppeltekens (-) bevatten, en mag niet beginnen of eindigen met een koppelteken.");
            return View();
        }
        
        if (_organisationManager.GetOrganisationById(name.ToLower()) != null)
        {
            ModelState.AddModelError("", "Deze organisatie bestaat al, kies een andere naam!");
            return View();
        }

        var organisation = _organisationManager.AddOrganisation(name.ToLower(), name, "#FFFFFF", "","", false);

        var subdomain = organisation.Name.ToLowerInvariant();
        var currentHost = HttpContext.Request.Host;
        var protocol = HttpContext.Request.Scheme;
        var baseDomain = currentHost.Host;

        if (baseDomain.StartsWith("www."))
        {
            baseDomain = baseDomain.Substring(4);
        }

        var parts = baseDomain.Split('.');
        if (parts.Length > 1)
        {
            baseDomain = $"{parts[parts.Length - 2]}.{parts[parts.Length - 1]}";
        }

        var port = currentHost.Port.HasValue ? $":{currentHost.Port}" : "";
        var newUrl = $"{protocol}://{subdomain}.{baseDomain}{port}/Identity/Account/Register";

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