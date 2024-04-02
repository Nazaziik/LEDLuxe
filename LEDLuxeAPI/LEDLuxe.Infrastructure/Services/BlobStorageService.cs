using Azure.Storage.Blobs;

namespace LEDLuxe.Infrastructure.Services;

public class BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
{
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly string _containerName = containerName;

    public async Task<string> UploadPhotoAsync(Stream photoStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(photoStream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public async Task DeletePhotoAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string> UpdatePhotoAsync(Stream newPhotoStream, string oldFileName, string newFileName)
    {
        if (!string.IsNullOrEmpty(oldFileName))
            await DeletePhotoAsync(oldFileName);

        return await UploadPhotoAsync(newPhotoStream, newFileName);
    }
}