namespace Restaurant.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(string fileName, Stream stream);
    string? GetBlobSasUrl(string? blobUrl);
}