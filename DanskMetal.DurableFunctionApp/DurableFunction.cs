using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DanskMetal_DurableFunctionApp
{
    public static class DurableFunction
    {
        [Function(nameof(DurableFunction))]
        public static async Task RunOrchestrator(
           [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(DurableFunction));
            logger.LogInformation("Saying hello.");
            using var timeoutCts = new CancellationTokenSource();
            DateTime dueTime = context.CurrentUtcDateTime.AddSeconds(10);
            logger.LogInformation("Should timeout at:{Time}", dueTime);
            Task durableTimeout = context.CreateTimer(dueTime, timeoutCts.Token);
            Task<bool> approvalEvent = context.WaitForExternalEvent<bool>("ApprovalEvent");
            if (approvalEvent == await Task.WhenAny(approvalEvent, durableTimeout))
            {
                timeoutCts.Cancel();
                System.Console.WriteLine($"Back from approver: {approvalEvent.Result}");
            }
            else
            {
                System.Console.WriteLine("Timeout");
                context.SetCustomStatus("Failed!!!!");
            }

        }


        [Function("DurableFunction_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("DurableFunction_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(DurableFunction));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }

     
    }
}
