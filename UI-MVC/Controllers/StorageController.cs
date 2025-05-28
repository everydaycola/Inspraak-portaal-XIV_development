using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

public class StorageController : Controller
{
    private readonly IStorageManager _storageManager;
    private readonly ILogger<StorageController> _logger;

    public StorageController(IStorageManager storageManager, ILogger<StorageController> logger)
    {
        _storageManager = storageManager;
        _logger = logger;
    }

    // GET: /Storage/GetFile?fileName=your-file-name.jpg
    public async Task<IActionResult> GetFile(string fileName)
    {
        try
        {
            var (fileStream, contentType, _) = await _storageManager.GetFileAsync(fileName);
            return File(fileStream, contentType);
        }
        catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
        {
            return NotFound();
        }
        catch (NullReferenceException e)
        {
            _logger.LogError("Failed to add file to bucket" + e.Message);
            return NotFound();
        }
    }
}