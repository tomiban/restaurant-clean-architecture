using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurant.Domain.Interfaces;
using Restaurant.Infrastructure.Configuration;

namespace Restaurant.Infrastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettings.Value;

    public async Task<string> UploadToBlobAsync(string fileName, Stream stream)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream);
        var url = blobClient.Uri.ToString();
        return url;
    }

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null)
            return null;
        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),
            BlobName = GetBlobUrl(blobUrl)
        };
        sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var sasToken = sasBuilder
            .ToSasQueryParameters(new StorageSharedKeyCredential(blobServiceClient.AccountName,
                _blobStorageSettings.AccountKey))
            .ToString();
        return $"{blobUrl}?{sasToken}";
    }

    public string GetBlobUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}