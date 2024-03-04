using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DanskMetal.FirstFunctionApp
{
    public class DanskMetalHttpTriggerFunction
    {
        private readonly ILogger<DanskMetalHttpTriggerFunction> _logger;

        public DanskMetalHttpTriggerFunction(ILogger<DanskMetalHttpTriggerFunction> logger)
        {
            _logger = logger;
        }

        [Function("DanskMetalHttpTriggerFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Dansk Metal Azure Functions!.");

            return response;
        }
    }
}
