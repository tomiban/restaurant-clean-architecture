namespace Restaurant.Infrastructure.Configuration;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = null!;
    public string LogosContainerName { get; set; } = null!;
    public string AccountKey { get; set; } = null!;
}