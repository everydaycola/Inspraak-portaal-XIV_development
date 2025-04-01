using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

public class QRCodeController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;
    private readonly IFileManager _fileManager;
    private readonly ICriteriaManager _criteriaManager;
    private readonly IPanelManager _panelManager;

    public QRCodeController(ILogger<PanelManagementController> logger, IFileManager fileManager, ICriteriaManager criteriaManager, IPanelManager panelManager)
    {
        _logger = logger;
        _fileManager = fileManager;
        _criteriaManager = criteriaManager;
        _panelManager = panelManager;
    }
    
    public IActionResult DownloadQrCodesForAllPanelMembers(Guid panelId)
    {
        var panelMembers = _panelManager.GetAllPanelMembersForPanel(panelId);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForMultiplePanelMembers(panelMembers, baseUrl);
        _logger.Log(LogLevel.Information, "Generating qr codes for all groups in panel{} ",panelId);
        return File(zipFileBytes, "application/zip", "qrcodes.zip");
    }
    public IActionResult DownloadQrCodesForSpecificCriteriaGroup(string groupName,ICollection<PanelMember> groupMembers)
    {
        foreach (var member in groupMembers)
        {
            Console.WriteLine(member.PanelMemberId);
        }
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForMultiplePanelMembers(groupMembers,baseUrl);
        _logger.Log(LogLevel.Information, "Generating qr codes for group : \'{1}\'",groupName);
        return File(zipFileBytes, "application/zip", $"qrcodes_{groupName}.zip");
    }
    public IActionResult DownloadSingleQrCode(string data)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(data);
        return File(qrCodeBytes, "image/png", "qrcode.png");
    }
}