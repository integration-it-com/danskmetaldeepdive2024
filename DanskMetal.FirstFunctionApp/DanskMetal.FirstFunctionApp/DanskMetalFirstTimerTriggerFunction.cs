using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DanskMetal.FirstFunctionApp
{
    public class DanskMetalFirstTimerTriggerFunction
    {
        private readonly ILogger _logger;

        public DanskMetalFirstTimerTriggerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DanskMetalFirstTimerTriggerFunction>();
        }

        //[Function("DanskMetalFirstTimerTriggerFunction")]
        public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Dansk Metal Timer Trigger executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

        }
    }
}
