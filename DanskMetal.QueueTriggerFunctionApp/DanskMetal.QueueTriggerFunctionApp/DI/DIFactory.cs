using deep_dive_first_function_app.Interfaces;
using deep_dive_first_function_app.Services;
using Microsoft.Extensions.DependencyInjection;

namespace deep_dive_first_function_app.DI
{

    public static class DIFactory
    {
        public static IServiceCollection AddFunctionAppServices(this IServiceCollection services)
        {
            services
                .AddScoped<IBlobHandler, BlobHandler>()

                .AddHttpClient()
          ;

            return services;
        }

        public static IServiceProvider GetProvider()
        {
            var services = new ServiceCollection();
            services
              .AddFunctionAppServices()
              ;
            return services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            var provider = GetProvider();
            return provider.GetRequiredService<T>();
        }
    }
}
