using System.Collections;
using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class PanelManagementController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;

    private readonly IPanelManager _manager;
    private readonly IFileManager _fileManager;
    private readonly ICriteriaManager _criteriaManager;
    
    
    public PanelManagementController(ILogger<PanelManagementController> logger, IPanelManager manager, IFileManager fileManager, ICriteriaManager criteriaManager)
    {
        _logger = logger;
        _manager = manager;
        _fileManager = fileManager;
        _criteriaManager = criteriaManager;
    }

    public IActionResult Index(Guid id)
    {
        var panel = _manager.GetPanelWithRepresentationGroup(id);
        var pmd = new PanelManagementDto(id, panel.Name, 20000, 0);
        pmd.PanelSize = _manager.CalculatePanelSize(pmd.CitizenCount, 0.005);
        pmd.AmountOfReserveInvites = _manager.CalculateAmountOfReserve(pmd.PanelSize, panel.RepresentationGroup.ReservePercentage);
        pmd.TotalInvitesNeeded = _manager.CalculateTotalInvitesNeeded(pmd.PanelSize + pmd.AmountOfReserveInvites, panel.RepresentationGroup.ResponseRate);
        pmd.IsRegistrationOpen = panel.IsRegistrationOpen;
        return View(pmd);
    }
    public IActionResult LoadUniqueCodes(Guid panelId)
    {
        var panel = _manager.GetPanelWithPanelMembersAndCriteria(panelId);
        var codes = PopulateUniqueCodesDto(panel);
        return PartialView("_UniqueCodesPartial", codes);
    }
    [HttpPost]
    public IActionResult SelectPanel(Guid panelId)
    {
        return RedirectToAction("Index", new { id = panelId });
    }
    public IActionResult PanelSelection()
    {
        var panels = _manager.GetAllPanels();
        return View(panels);
    }
    
    public IActionResult DownloadQrCodesForAllGroups(Guid panelId)
    {
        var criteriaGroups = _criteriaManager.GetAllCriteriaGroupForPanel(panelId);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForAllCodesInAllGroups(criteriaGroups, baseUrl);
        return File(zipFileBytes, "application/zip", "qrcodes.zip");
    }
    public IActionResult DownloadQrCodesForSpecificGroup(Guid panelId, string groupName)
    {
        var group = _criteriaManager.GetCriteriaGroupByPanelIdAndName(panelId, groupName);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForAllCodesInAGroup(group,baseUrl);
        return File(zipFileBytes, "application/zip", $"qrcodes_{groupName}.zip");
    }
    public IActionResult DownloadSingleQrCode(string generatedUrl)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(generatedUrl);
        return File(qrCodeBytes, "image/png", "qrcode.png");
    }
    
    private List<GroupedUniqueCodesDto> PopulateUniqueCodesDto(Panel panel)
    {
        var codes = new List<UniqueCodesDto>();
        foreach (var member in panel.PanelMembers)
        {
            var codeDto = new UniqueCodesDto(panel.Id, member.PanelMemberId, member.CriteriaGroup);
            foreach (var criteria in member.CriteriaGroup.Criteria)
            {
                codeDto.Criteria.Add(criteria);
            }
            codes.Add(codeDto);
        }
        
        return codes.GroupBy(entry => entry.CriteriaGroup.Id)
            .Select(group => new GroupedUniqueCodesDto()
            {
                GroupKey= group.Key.ToString(),
                Members = group.ToList(),
                Name = group.First().CriteriaGroup.Name
            }).ToList();
    }
    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }
   
}
