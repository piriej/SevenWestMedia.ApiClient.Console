using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
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
            AddMockConfig(serviceCollection);
            serviceCollection
                .AddTransient<IAggregateServices<Person, PeopleViewModel>, ServiceAggregator<Person, PeopleViewModel>>();
            serviceCollection.AddTransient<ISampleTestService, SampleTestService>();
            ScanFixtureBuilders(serviceCollection);
            
            AddPolicyRegistry(serviceCollection);
            serviceCollection.AddTransient(typeof(IMapper<Person, PeopleViewModel>), typeof(ViewModelMapper));
            var x = serviceCollection.BuildServiceProvider();
            return x;
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

        private static void AddPolicyRegistry(ServiceCollection services)
        {
            var registry = services.AddPolicyRegistry();

            registry.Add("RetryPolicy", PolicyRepository.RetryPolicy());
            registry.Add("CircuitBreakerPolicy", PolicyRepository.CircuitBreakerPolicy());

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Nothing"),
                })
                .Verifiable();

            services.AddHttpClient<ISampleTestService, SampleTestService>((serviceCollection, client) =>
                {
                    client.BaseAddress = new Uri("http://gonowhere.com");
                    client.Timeout = new TimeSpan(10000);
                })
                .ConfigurePrimaryHttpMessageHandler(sp => handlerMock.Object)
                .AddPolicyHandlerFromRegistry("RetryPolicy")
                .AddPolicyHandlerFromRegistry("CircuitBreakerPolicy");

        }
    }
}