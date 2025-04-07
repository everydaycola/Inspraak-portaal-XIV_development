using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class OrganisationManagementController : Controller
{
    private IOrganisationManager _organisationManager;

    public OrganisationManagementController(IOrganisationManager organisationManager)
    {
        _organisationManager = organisationManager;
    }

    public IActionResult Index(string organisationId)
    {
        var organisation = _organisationManager.GetOrganisationById(organisationId);
        return View(organisation);
    }
    public IActionResult Edit(string organisationId)
    {
        var organisation = _organisationManager.GetOrganisationById(organisationId);
        ViewBag.IsEditing = true;
        return View("Index",organisation);
    }
    [HttpPost]
    public IActionResult Update(string organisationId, string Name, string BackgroundColor, string BackgroundImage)
    {
        var organisation = _organisationManager.UpdateOrganisation(organisationId, Name, BackgroundColor,BackgroundImage);
        
        ViewBag.IsEditing = false;
        return View("Index",organisation);
    }
}