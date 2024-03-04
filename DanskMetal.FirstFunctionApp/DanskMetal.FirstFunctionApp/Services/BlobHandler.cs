using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using deep_dive_first_function_app.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace deep_dive_first_function_app.Services
{
    public class BlobHandler : IBlobHandler
    {
        private readonly ILogger<BlobHandler> _logger;

        public BlobHandler(ILogger<BlobHandler> logger)
        {
            _logger = logger;
        }
        public async Task SaveBlobAsync(string contents, string blobName)
        {
            var uri = new Uri($"https://blobcontainerst.blob.core.windows.net/output/{blobName}");
            var credentials = GetCredential();
            var client = new BlobClient(blobUri: uri, credential: credentials);
            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            await client.UploadAsync(blobStream);
        }

        private TokenCredential GetCredential()
        {
            TokenCredential credentials;
            _ = bool.TryParse(Environment.GetEnvironmentVariable("RunningLocally"), out bool runningLocally);
            if (runningLocally)
            {
                credentials = new AzureCliCredential();
            }
            else
            {
                credentials = new DefaultAzureCredential();
            }
            return credentials;
        }
    }
}
