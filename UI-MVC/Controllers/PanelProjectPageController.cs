using BL.Interfaces;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto.ProjectPage;

namespace UI_MVC.Controllers;

public class PanelProjectPageController: Controller
{
    
    private readonly ILogger<PanelProjectPageController> _logger;
    private readonly IPanelManager _panelManager;
    private readonly IStorageManager _storageManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomUserManager _customUserManager;

    public PanelProjectPageController(ILogger<PanelProjectPageController> logger, IPanelManager panelManager, IStorageManager storageManager, ICustomUserManager customUserManager, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _panelManager = panelManager;
        _storageManager = storageManager;
        _customUserManager = customUserManager;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "Organisatie,PanelMember")]
    public async Task<IActionResult> Index(Guid? PanelId)
    {
        if (!PanelId.HasValue)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            PanelId = _customUserManager.getPanelForUser(user.Id).Id;
        }
        var panel = _panelManager.GetPanelWithPosts(PanelId.Value);
        var projectPageDto = new ProjectPageDto
        {
            Panel = panel,
        };
        return View(projectPageDto);
    }

    [HttpPost]
    public IActionResult AddTextPost(Guid panelId, string title,string content, bool visibleForPanelMember)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ModelState.AddModelError("", "Content cannot be null");
            var panel = _panelManager.GetPanelWithPosts(panelId);
            var projectPageDto = new ProjectPageDto
            {
                Panel = panel
            };
            return View("Index", projectPageDto); 
        }
        
        _panelManager.AddTextPost(panelId, title,content, visibleForPanelMember);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId); 
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };
        return View("Index", updatedProjectPageDto); 
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDocumentPost(string title, IFormFile file, Guid panelId, bool visibleForPanelMember)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("file", "Geen bestand geselecteerd.");
            return RedirectToAction("ProjectPage", new { id = panelId });
        }

        // TEMP Create a folder path for uploads
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        //Generate a unique filename
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        // TEMP Save the file localy
        await _storageManager.AddFileAsync(uniqueFileName, file.ContentType, file.OpenReadStream());
        //SAVE META DATA IN DB
        _panelManager.AddDocumentPost(panelId,title,uniqueFileName, visibleForPanelMember);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId); 
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };
        return View("Index", updatedProjectPageDto); 
    }
}