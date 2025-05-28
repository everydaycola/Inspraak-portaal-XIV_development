using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;

public class OrganisationManagementController : Controller
{
    private readonly IOrganisationManager _organisationManager;
    private readonly ILogger<PanelManagementController> _logger;
    private readonly IStorageManager _storageManager;

    public OrganisationManagementController(IOrganisationManager organisationManager,
        ILogger<PanelManagementController> logger, IStorageManager storageManager)
    {
        _organisationManager = organisationManager;
        _logger = logger;
        _storageManager = storageManager;
    }

    //Onderstaande views behoren tot het beheren van organisaties door Admin accounts.

    [Authorize(Roles = CustomIdentityConstants.AdminRole)]
    public IActionResult AdminIndex()
    {
        var organisationsDto = new OrganisationsViewmodel
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
            var updatedOrg = _organisationManager.UpdateOrganisation(organisationId, name, org.BackgroundColor,
                org.BackgroundImage, org.LogoImageName, org.IsTextColorWhite);
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
        return View("Index", organisation);
    }

    [HttpPost]
    public async Task<IActionResult> Update(string organisationId, OrganisationManagementViewModel model)
    {
        var uniqueFileName = "";
        if (model.File != null)
        {
            uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.File.FileName);
            try
            {
                await _storageManager.AddFileAsync(uniqueFileName, model.File.ContentType, model.File.OpenReadStream());
            }
            catch (NullReferenceException e)
            {
                _logger.LogError("Failed to add file to bucket" + e.Message);
            }
        }

        var uniqueFileNameLogo = "";
        if (model.LogoFile != null)
        {
            uniqueFileNameLogo = Guid.NewGuid() + Path.GetExtension(model.LogoFile.FileName);
            try
            {
                await _storageManager.AddFileAsync(uniqueFileNameLogo, model.LogoFile.ContentType,
                    model.LogoFile.OpenReadStream());
            }
            catch (NullReferenceException e)
            {
                _logger.LogError("Failed to add file to bucket" + e.Message);
            }
        }

        var organisation = _organisationManager.UpdateOrganisation(organisationId, model.Name, model.BackgroundColor,
            uniqueFileName, uniqueFileNameLogo, model.IsTextColorWhite);

        ViewBag.IsEditing = false;
        return View("Index", organisation);
    }
}