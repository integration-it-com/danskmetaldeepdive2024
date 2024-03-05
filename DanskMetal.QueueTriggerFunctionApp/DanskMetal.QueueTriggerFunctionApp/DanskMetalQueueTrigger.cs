using System;
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
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
