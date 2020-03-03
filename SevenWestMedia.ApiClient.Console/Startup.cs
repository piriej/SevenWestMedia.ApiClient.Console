using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SevenWestMedia.ApiClient.Console.ViewModels;
using SevenWestMedia.ApiClient.Library.Models;
using SevenWestMedia.ApiClient.Library.ServiceAdapter;

namespace SevenWestMedia.ApiClient.Console
{
    public class Startup
    {
        private IServiceProvider ServicesProvider { get; set; }
        public IConfigurationRoot Configuration { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services = null)
        {
            if (services == null)
                services = new ServiceCollection();

            services.AddLogging(config => config.AddConsole());
            AddConfiguration(services);

            services.AddTransient(typeof(IMapper<Person, PeopleViewModel>), typeof(ViewModelMapper));
            services.AddTransient<IAggregateServices<Person, PeopleViewModel>, ServiceAggregator<Person, PeopleViewModel>>();
            services.AddTransient<ISampleTestService, SampleTestService>();
            services.AddSwmApiClientLibrary();

            ServicesProvider = services.BuildServiceProvider();

            return ServicesProvider;
        }

        public void Configure()
        {
            var logger = ServicesProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");
        }

        private void AddConfiguration(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            services.AddSingleton<IConfiguration>(Configuration);
        }
    }
}