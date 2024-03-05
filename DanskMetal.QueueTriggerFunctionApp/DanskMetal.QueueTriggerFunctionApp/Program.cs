using deep_dive_first_function_app.DI;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddFunctionAppServices();
    })
    .Build();

host.Run();
