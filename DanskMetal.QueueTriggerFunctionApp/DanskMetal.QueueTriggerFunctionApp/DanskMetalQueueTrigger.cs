using System;
using System.Reflection.Metadata;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DanskMetal.QueueTriggerFunctionApp
{
    public class DanskMetalQueueTrigger
    {
        private readonly ILogger<DanskMetalQueueTrigger> _logger;

        public DanskMetalQueueTrigger(ILogger<DanskMetalQueueTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(DanskMetalQueueTrigger))]
        public void Run([QueueTrigger("triggerqueue", Connection = "BlobContainerConnectionString")] QueueMessage message)
        {

            _logger.LogInformation("C# Queue trigger function Processed processed message in queue");
            var newFileMetaData = JsonSerializer.Deserialize<AzureFunctionAppConfiguration>(message.Body);

            
            //string output = Reverse(input);
            //_logger.LogInformation("The reversed output was {Output}", output);
            //string outputBlobName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
            //await _blobHandler.SaveBlobAsync(output, outputBlobName);


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
            public string File { get; set; } = string.Empty;
        }
    }
}
