using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.Dto.OrganisationDtos;

namespace UI_MVC.Controllers;

public class OrganisationManagementController : Controller
{
    private readonly IOrganisationManager _organisationManager;
    private readonly ILogger<PanelManagementController> _logger;

    public OrganisationManagementController(IOrganisationManager organisationManager, ILogger<PanelManagementController> logger)
    {
        _organisationManager = organisationManager;
        _logger = logger;
    }
    
    //Onderstaande views behoren tot het beheren van organisaties door Admin accounts.

    [Authorize(Roles = CustomIdentityConstants.AdminRole)]
    public IActionResult AdminIndex()
    {
        var organisationsDto = new OrganisationManagementDto
        {
            Organisations = _organisationManager.GetAllOrganisations(),
            AmountOfOrganisations = _organisationManager.GetAllOrganisations().Count()
        };
        return View(organisationsDto);
    }
    [Authorize(Roles = CustomIdentityConstants.AdminRole)]
    [HttpPost]
    public IActionResult AdminOrganisationUpdate(string organisationId, string name)
    {
        var org = _organisationManager.GetOrganisationById(organisationId);
        try
        {
            var updatedOrg = _organisationManager.UpdateOrganisation(organisationId, name, org.BackgroundColor, org.BackgroundImage);
            var responseDto = new OrganisationDto
            {
                Id = updatedOrg.Id,
                Name = updatedOrg.Name
            };
            return Ok(responseDto);
        }
        catch
        {
            _logger.Log(LogLevel.Critical, "Organisation update failed, no organisation with ID {organisationId}");
            return NotFound(new { message = "Organisation not found." });
        }
    }
    [Authorize(Roles = CustomIdentityConstants.AdminRole)]
    [HttpPost]
    public IActionResult AdminOrganisationDelete(string organisationId)
    {
        _organisationManager.DeleteOrganisation(organisationId);
        return RedirectToAction("AdminIndex");
    }
    
    //Onderstaande views behoren tot de pagina voor organisaties zelf
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