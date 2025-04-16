using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
    private readonly ILogger<StorageController> _logger;

    public StorageController(ILogger<StorageController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetFile(string fileName)
    {
        // Acts as me
        var client = StorageClient.Create();
        // Where to keep the file
        var stream = new MemoryStream();
        // Downloading the file - Downloads into the Stream, not into the obj
        var obj = await client.DownloadObjectAsync("development-164899", fileName, stream);
        stream.Position = 0;
        return File(stream, obj.ContentType, obj.Name);
    }

    [HttpPost]
    public async Task<IActionResult> PostFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        using var stream = file.OpenReadStream();

        // Acts as me
        var client = StorageClient.Create();

        var obj = await client.UploadObjectAsync(
            bucket: "development-164899",
            objectName: file.FileName,
            contentType: file.ContentType,
            source: stream);
        return Ok(new { obj.Name, obj.ContentType, obj.Size });
    }
}