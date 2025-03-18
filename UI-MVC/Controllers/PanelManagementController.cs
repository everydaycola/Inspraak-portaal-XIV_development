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
        PanelManagementDto pmd = new PanelManagementDto(id, panel.Name, 20000, 0);
        pmd.SuccesfulRegistrationCount = panel.SuccesfulRegistrationCount;
        pmd.PanelSize = _manager.CalculatePanelSize(pmd.CitizenCount, 0.005);
        pmd.AmountOfReserveInvites = _manager.CalculateAmountOfReserve(pmd.PanelSize, panel.RepresentationGroup.ReservePercentage);
        pmd.TotalInvitesNeeded = _manager.CalculateTotalInvitesNeeded(pmd.PanelSize + pmd.AmountOfReserveInvites, panel.RepresentationGroup.ResponseRate);
        pmd.IsRegistrationOpen = panel.IsRegistrationOpen;
        return View(pmd);
    }
    public IActionResult LoadUniqueCodes(Guid panelId)
    {
        Panel panel = _manager.GetPanelWithPanelMembersAndCriteria(panelId);
        var codes = populateUniqueCodesDto(panel);
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
    
    public IActionResult DownloadQRCodesForAllGroups(Guid panelId)
    {
        var criteriaGroups = _criteriaManager.GetAllCriteriaGroupForPanel(panelId);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForAllCodesInAllGroups(criteriaGroups, baseUrl);
        return File(zipFileBytes, "application/zip", "qrcodes.zip");
    }
    public IActionResult DownloadQRCodesForSpecificGroup(Guid panelId, string groupName)
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
    
    private List<GroupedUniqueCodesDto> populateUniqueCodesDto(Panel panel)
    {
        ICollection<UniqueCodesDto> codes = new List<UniqueCodesDto>();

        foreach (var member in panel.PanelMembers)
        {
            if (member.CriteriaGroup == null)
            {
                var codeDto = new UniqueCodesDto(panel.Id, member.PanelMemberId, null)
                {
                    criteria = new List<Criteria>() 
                };
                codes.Add(codeDto);
            }
            else
            {
                var codeDto = new UniqueCodesDto(panel.Id, member.PanelMemberId, member.CriteriaGroup);
                foreach (var criteriaAnswer in member.CriteriaGroup.CriteriaAnswers)
                {
                    codeDto.criteria.Add(criteriaAnswer.criteria);
                }
                codes.Add(codeDto);
            }
        }
        
        var groupedCodes = codes.GroupBy(entry => entry.CriteriaGroup?.Id ?? Guid.Empty) // Gebruik Guid.Empty voor onbekende groepen
            .Select(group => new GroupedUniqueCodesDto()
            {
                GroupKey = group.Key == Guid.Empty ? "Onbekend" : group.Key.ToString(),
                Members = group.ToList(),
                Name = group.Key == Guid.Empty ? "Criteria onbekend" : group.First().CriteriaGroup?.Name
            })
            .ToList();
        
        return groupedCodes.OrderByDescending(group => group.Name == "Criteria onbekend").ToList();
    }
    public IActionResult ToggleRegistration(Guid panelId)
    {
        var panel = _manager.GetPanel(panelId);
        _manager.UpdatePanel(panelId, !panel.IsRegistrationOpen);
        return RedirectToAction("Index", new { id = panel.Id });
    }
   
}
