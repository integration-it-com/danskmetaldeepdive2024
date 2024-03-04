namespace deep_dive_first_function_app.Interfaces
{
    public interface IBlobHandler
    {
        Task SaveBlobAsync(string contents, string blobName);
    }
}
