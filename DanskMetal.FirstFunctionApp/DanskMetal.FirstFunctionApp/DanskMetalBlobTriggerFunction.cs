using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DanskMetal.FirstFunctionApp
{
    public class DanskMetalBlobTriggerFunction
    {
        private readonly ILogger<DanskMetalBlobTriggerFunction> _logger;

        public DanskMetalBlobTriggerFunction(ILogger<DanskMetalBlobTriggerFunction> logger)
        {
            _logger = logger;
        }

        
        [Function("DanskMetalBlobTriggerFunction")]
        public async Task Run(
            [BlobTrigger("upload/{name}", Connection = "DanskMetalConnectionString")] string input,
            [BlobInput("config/config.txt", Connection = "DanskMetalConnectionString")] Stream configStream,
            string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {input}");
            var config = JsonSerializer.Deserialize<AzureFunctionAppConfiguration>(configStream);
            
            string output = Reverse(input);
            _logger.LogInformation($"The reversed output was {output}", output);
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
