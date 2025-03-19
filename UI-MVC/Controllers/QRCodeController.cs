using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

public class QRCodeController : Controller
{
    private readonly ILogger<PanelManagementController> _logger;
    private readonly IFileManager _fileManager;
    private readonly ICriteriaManager _criteriaManager;

    public QRCodeController(ILogger<PanelManagementController> logger, IFileManager fileManager, ICriteriaManager criteriaManager)
    {
        _logger = logger;
        _fileManager = fileManager;
        _criteriaManager = criteriaManager;
    }
    
    public IActionResult DownloadQrCodesForAllCriteriaGroups(Guid panelId)
    {
        var criteriaGroups = _criteriaManager.GetAllCriteriaGroupForPanel(panelId);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForAllCodesInAllGroups(criteriaGroups, baseUrl);
        _logger.Log(LogLevel.Information, "Generating qr codes for all groups in panel{} ",panelId);
        return File(zipFileBytes, "application/zip", "qrcodes.zip");
    }
    public IActionResult DownloadQrCodesForSpecificCriteriaGroup(Guid panelId, string groupName)
    {
        var group = _criteriaManager.GetCriteriaGroupByPanelIdAndName(panelId, groupName);
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Register";
        var zipFileBytes = _fileManager.CreateZipFileForAllCodesInAGroup(group,baseUrl);
        _logger.Log(LogLevel.Information, "Generating qr codes for group : \'{1}\' in panel{2} ",panelId, groupName);
        return File(zipFileBytes, "application/zip", $"qrcodes_{groupName}.zip");
    }
    public IActionResult DownloadSingleQrCode(string data)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(data);
        return File(qrCodeBytes, "image/png", "qrcode.png");
    }
}