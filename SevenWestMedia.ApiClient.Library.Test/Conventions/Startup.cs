using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using SevenWestMedia.ApiClient.Console.ViewModels;
using SevenWestMedia.ApiClient.Library.Configuration;
using SevenWestMedia.ApiClient.Library.Models;
using SevenWestMedia.ApiClient.Library.ServiceAdapter;

namespace SevenWestMedia.ApiClient.Library.Test.Conventions
{
    public static class Startup
    {
        public static ServiceProvider Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSwmApiClientLibrary();
            ScanFixtureBuilders(serviceCollection);
            AddMockConfig(serviceCollection);
            serviceCollection.AddTransient(typeof(IMapper<Person, PeopleViewModel>), typeof(ViewModelMapper));
            return serviceCollection.BuildServiceProvider();
        }

        private static void ScanFixtureBuilders(ServiceCollection serviceCollection)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.Name == "IBuildFixtures`1"))
                .ToList();

            foreach (var fixtureBuilder in types)
            {
                serviceCollection.AddTransient(fixtureBuilder.GetGenericTypeDefinition());
            }
        }

        private static void AddMockConfig(ServiceCollection services)
        {
            var config = new Config();
            var mock = new Mock<IOptions<Config>>();
            mock.Setup(c => c.Value).Returns(config);
            services.AddSingleton(mock.Object);
        }
        
    }
}