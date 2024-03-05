using deep_dive_first_function_app.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DanskMetal.FirstFunctionApp
{
    public class DanskMetalBlobTriggerFunction
    {
        private readonly ILogger<DanskMetalBlobTriggerFunction> _logger;
        private readonly IBlobHandler _blobHandler;

        public DanskMetalBlobTriggerFunction(ILogger<DanskMetalBlobTriggerFunction> logger, IBlobHandler blobHandler)
        {
            _logger = logger;
            _blobHandler = blobHandler;
        }

        // Remember to upload the config.txt file to the config container in the storage account
        [Function("DanskMetalBlobTriggerFunction")]
        public async Task Run(
            [BlobTrigger("upload/{name}", Connection = "DanskMetalConnectionString")] string input,
            [BlobInput("config/config.txt", Connection = "DanskMetalConnectionString")] Stream configStream,
            string name)
        {


            _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {Name} \n Data: {Input}", name, input);
            var config = JsonSerializer.Deserialize<AzureFunctionAppConfiguration>(configStream);

            string output = Reverse(input);
            _logger.LogInformation("The reversed output was {Output}", output);
            string outputBlobName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
            await _blobHandler.SaveBlobAsync(output, outputBlobName);

        }

        private string Reverse(string input)
        {
            char[] reversedArray = input.ToCharArray();
            Array.Reverse(reversedArray);
            string output = new(reversedArray);
            return output;
        }
        private class AzureFunctionAppConfiguration
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}
