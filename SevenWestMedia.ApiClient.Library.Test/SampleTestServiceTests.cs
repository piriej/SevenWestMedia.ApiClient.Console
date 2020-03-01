using System.Linq;
using System.Net;
using FluentAssertions;
using SevenWestMedia.ApiClient.Library.Configuration;
using SevenWestMedia.ApiClient.Library.ServiceAdapter;
using SevenWestMedia.ApiClient.Library.Test.Conventions;

namespace SevenWestMedia.ApiClient.Library.Test
{
    public class SampleTestServiceTests
    {
        public void CanDeserializeARecordFromTheService(
            HttpClientFixtureBuilder<Person> httpClientFixtureBuilder,
            Config config)
        {
            const string firstName = "Jeff";

            var httpClient = httpClientFixtureBuilder
                .WithStatusCode(HttpStatusCode.OK)
                .WithModel(model => model.With(p => p.FirstName, firstName))
                .WithGet()
                .Build();

            var service = new SampleTestService(httpClient, config);
            var people = service.DeserialiseStream<Person>();
            people.Result.Count().Should().Be(1);
            people.Result.First().FirstName.Should().Be(firstName);
        }
    }
}
