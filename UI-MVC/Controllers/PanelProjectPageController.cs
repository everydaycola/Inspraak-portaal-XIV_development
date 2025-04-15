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

    public PanelProjectPageController(ILogger<PanelProjectPageController> logger, IPanelManager panelManager)
    {
        _logger = logger;
        _panelManager = panelManager;
    }
    
    [Authorize(Roles = "Organisatie")]
    public IActionResult Index(Guid PanelId)
    {
        var panel = _panelManager.GetPanelWithPosts(PanelId);
        var projectPageDto = new ProjectPageDto
        {
            Panel = panel,
        };
        return View(projectPageDto);
    }

    [HttpPost]
    public IActionResult AddTextPost(Guid panelId, string content)
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

        _panelManager.AddTextPost(panelId, content);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId); 
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };
        return View("Index", updatedProjectPageDto); 
    }
    
    [HttpPost]
    public async Task<IActionResult> AddImagePost(IFormFile imageFile, Guid panelId)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError("imageFile", "Geen bestand geselecteerd.");
            return RedirectToAction("ProjectPage", new { id = panelId });
        }

        // TEMP Create a folder path for uploads
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        //Generate a unique filename
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
        // TEMP Save the file localy
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }
        //SAVE META DATA IN DB
        string documentUrl = "/uploads/"+uniqueFileName;
        _panelManager.AddDocumentPost(panelId, documentUrl);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId); 
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };
        return View("Index", updatedProjectPageDto); 
    }
}