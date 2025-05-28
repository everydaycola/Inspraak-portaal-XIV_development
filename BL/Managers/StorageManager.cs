using BL.Interfaces;
using BL.Options;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BL.Managers;

public class StorageManager : IStorageManager
{
    private readonly ILogger<StorageManager> _logger;
    private readonly GoogleCloudOptions _googleCloudOptions;
    private readonly StorageClient _storageClient;

    public StorageManager(ILogger<StorageManager> logger, IOptions<GoogleCloudOptions> googleCloudOptions)
    {
        _logger = logger;
        try
        {
            _storageClient = StorageClient.Create();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Failed to connect to storage client");
        }
        _googleCloudOptions = googleCloudOptions.Value;
    }

    public async Task<(Stream FileStream, string ContentType, string FileName)> GetFileAsync(string fileName)
    {
        var stream = new MemoryStream();
        var obj = await _storageClient.DownloadObjectAsync(
            bucket: _googleCloudOptions.BucketName,
            objectName: fileName,
            destination: stream);
        stream.Position = 0;
        return (stream, obj.ContentType, obj.Name);
    }

    public async Task<(string Name, string ContentType, long Size)> AddFileAsync(string fileName, string contentType, Stream fileStream)
    {
        var obj = await _storageClient.UploadObjectAsync(
            bucket: _googleCloudOptions.BucketName,
            objectName: fileName,
            contentType: contentType,
            source: fileStream);
        return (obj.Name, obj.ContentType, (long) obj.Size);
    }
}