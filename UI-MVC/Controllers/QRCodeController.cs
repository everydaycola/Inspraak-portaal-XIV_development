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
    /*public IActionResult DownloadQrCodesForSpecificCriteriaGroup(Guid criteriaGroupId)
    {
        var group = _criteriaManager.GetCriteriaGroupById(criteriaGroupId);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForMultiplePanelMembers(group,baseUrl);
        _logger.Log(LogLevel.Information, "Generating qr codes for group : \'{1}\'",group.Name);
        return File(zipFileBytes, "application/zip", $"qrcodes_{group.Name}.zip");
    }*/
    public IActionResult DownloadSingleQrCode(string data)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(data);
        return File(qrCodeBytes, "image/png", "qrcode.png");
    }
}