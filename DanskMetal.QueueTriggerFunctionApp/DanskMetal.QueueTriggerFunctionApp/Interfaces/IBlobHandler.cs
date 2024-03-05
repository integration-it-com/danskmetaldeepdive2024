namespace deep_dive_first_function_app.Interfaces
{
    public interface IBlobHandler
    {
        Task SaveBlobAsync(string contents, string blobName, string storageAccountName = "blobcontainerst");
        Task<string> GetBlobAsync(string blobName, string storageAccountName = "blobcontainerst", string containerName = "upload");


    }
}
