using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SevenWestMedia.ApiClient.Library.Configuration;

namespace SevenWestMedia.ApiClient.Library.ServiceAdapter
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSwmApiClientLibrary(this IServiceCollection services)
        {
            AddOptions(services);
            AddHttpClient(services);

            return services;
        }

        private static void AddOptions(IServiceCollection services)
        {
            services.AddOptions<Config>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Config").Bind(settings); });
        }

        private static void AddHttpClient(IServiceCollection services)
        {
            // Pool HTTP connections with HttpClientFactory to prevent thread pool starvation,
            // Register SampleTestService as a Typed Service Agent to use HttpClient.
            // Ensure that intermittent issues are handled with a retry policy, and spamming prevented with a circuit breaker.
            // Delay recycling clients to every 5 mins (So every 5 mins guaranteed to hit DNS)
            services.AddHttpClient<ISampleTestService, SampleTestService>((serviceCollection, client) =>
                {
                    var config = serviceCollection.GetService<Config>();
                    client.BaseAddress = new Uri(config.BaseAddress);
                })
                .AddPolicyHandler(Policies.RetryPolicy())
                .AddPolicyHandler(Policies.CircuitBreakerPolicy())
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        }
    }
}