using deep_dive_first_function_app.DI;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {

        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(s =>
            {
                s.AddFunctionAppServices();
            })
            .Build();

        host.Run();

    }
}