using BL.Interfaces;
using Domain;
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
}