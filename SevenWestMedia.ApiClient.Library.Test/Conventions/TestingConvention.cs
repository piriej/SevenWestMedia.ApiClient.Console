using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SevenWestMedia.ApiClient.Library.ServiceAdapter;

namespace SevenWestMedia.ApiClient.Library.Test.Conventions
{
    using Fixie;

    public class TestingConvention : Discovery
    {
        public TestingConvention()
        {
            // Setup the container;
            var services = Setup();

            // Use the container as a parameter source for the tests.
            var inputParameterSource = new InputParameterSource(services);

            Classes
                .Where(x => x.Name.EndsWith("Tests"));

            Methods
                .Where(x => x.Name != "TestSetup");

            Parameters
                .Add(inputParameterSource);
        }

        private static ServiceProvider Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSwmApiClientLibrary();
            ScanFixtureBuilders(serviceCollection);
            AddMockConfig(serviceCollection);
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
            var configRoot = new Mock<IConfigurationRoot>();

            configRoot
                .SetupGet(x => x[It.IsAny<string>()])
                .Returns("AnyResult");

            services.AddSingleton(typeof(IConfigurationRoot), configRoot);
        }
    }
}

