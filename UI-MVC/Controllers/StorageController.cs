using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI_MVC.Options;

namespace UI_MVC.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
    private readonly ILogger<StorageController> _logger;
    private readonly GoogleCloudOptions _googleCloudOptions;

    public StorageController(ILogger<StorageController> logger, IOptions<GoogleCloudOptions> googleCloudOptions)
    {
        _logger = logger;
        _googleCloudOptions = googleCloudOptions.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetFile(string fileName)
    {
        // Simple check needs to change but works in development. You guys can do with this what you deem to be best
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("Filename must be provided.");
        }
        try
        {
            var client = StorageClient.Create();
            var stream = new MemoryStream();

            var obj = await client.DownloadObjectAsync(
                bucket: _googleCloudOptions.BucketName,
                objectName: fileName,
                destination: stream);

            stream.Position = 0;

            return File(stream, obj.ContentType, obj.Name);
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
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
            bucket: _googleCloudOptions.BucketName,
            objectName: file.FileName,
            contentType: file.ContentType,
            source: stream);
        return Ok(new { obj.Name, obj.ContentType, obj.Size });
    }
}