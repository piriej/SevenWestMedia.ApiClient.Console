using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SevenWestMedia.ApiClient.Console
{
    public class Startup
    {
        private IServiceProvider ServicesProvider { get; set; }
        public IConfigurationRoot Configuration { get; private set; }

        public Startup ConfigureServices(IServiceCollection services = null)
        {
            if (services == null)
                services = new ServiceCollection();

            services.AddLogging(config => config.AddConsole());
            AddConfiguration(services);

            ServicesProvider = services.BuildServiceProvider();

            return this;
        }

        public Startup Configure()
        {
            var logger = ServicesProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");
            return this;
        }

        private void AddConfiguration(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            services.AddSingleton(Configuration);
        }
    }
}