namespace BL.Interfaces;

public interface IStorageManager
{
    Task<(Stream FileStream, string ContentType, string FileName)> GetFileAsync(string fileName);

    Task<(string Name, string ContentType, long Size)> AddFileAsync(string fileName, string contentType,
        Stream fileStream);
}