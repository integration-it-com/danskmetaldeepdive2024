using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DanskMetal_DockerFunctionApp
{
    public class DockerHttpFunction
    {
        private readonly ILogger<DockerHttpFunction> _logger;

        public DockerHttpFunction(ILogger<DockerHttpFunction> logger)
        {
            _logger = logger;
        }

        [Function("DockerHttpFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Dansk Metal Docker Azure Functions!.");

            return response;
        }
    }
}
