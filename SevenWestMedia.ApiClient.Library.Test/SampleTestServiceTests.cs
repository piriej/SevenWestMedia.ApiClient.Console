using System.Linq;
using System.Net;
using FluentAssertions;
using SevenWestMedia.ApiClient.Console.ViewModels;
using SevenWestMedia.ApiClient.Library.Configuration;
using SevenWestMedia.ApiClient.Library.Models;
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

        public void ServiceAggregatorCorrectlyAggregatesNamesForId42(
            HttpClientFixtureBuilder<Person> httpClientFixtureBuilder,
            IMapper<Person, PeopleViewModel> viewModelMapper,
            Config config)
        {
            var httpClient = httpClientFixtureBuilder
                .WithStatusCode(HttpStatusCode.OK)
                .WithModel(model => model.With(p => p.Id, 42))
                .WithGet()
                .Build();

            var service = new SampleTestService(httpClient, config);
            var serviceAggregator = new ServiceAggregator<Person, PeopleViewModel>(service);
            var vm = serviceAggregator.AggregateOperation(viewModelMapper.Map).Result;

            vm.Id.Should().Be(42);

            var firstname = httpClientFixtureBuilder.HttpBodyModel.FirstOrDefault()?.FirstName;
            var lastname = httpClientFixtureBuilder.HttpBodyModel.FirstOrDefault()?.LastName;
            vm.FullName.Should().Be($"{firstname} {lastname}");
        }

        public void ServiceAggregatorCorrectlyAggregatesFirstNamesAged23(
            HttpClientFixtureBuilder<Person> httpClientFixtureBuilder,
            IMapper<Person, PeopleViewModel> viewModelMapper,
            Config config)
        {
            var httpClient = httpClientFixtureBuilder
                .WithStatusCode(HttpStatusCode.OK)
                .WithAllModel(model => model.With(p => p.Age, 23), 3)
                .WithGet()
                .Build();

            var service = new SampleTestService(httpClient, config);
            var serviceAggregator = new ServiceAggregator<Person, PeopleViewModel>(service);
            var vm = serviceAggregator.AggregateOperation(viewModelMapper.Map).Result;

            vm.Age.Should().Be(23);

            var firstNames = httpClientFixtureBuilder.HttpBodyModel.Select(x => x.FirstName).ToList();
            vm.FirstNames.Should().BeEquivalentTo(firstNames);
        }

        public void ServiceAggregatorCorrectlyAggregatesNumberOfGendersPerAge(
            HttpClientFixtureBuilder<Person> httpClientFixtureBuilder,
            IMapper<Person, PeopleViewModel> viewModelMapper,
            Config config)
        {
            var peopleMock = PeopleMock.WithMultipleGendersAndAges();

            var httpClient = httpClientFixtureBuilder
                .WithStatusCode(HttpStatusCode.OK)
                .WithModel(peopleMock)
                .WithGet()
                .Build();

            var service = new SampleTestService(httpClient, config);
            var serviceAggregator = new ServiceAggregator<Person, PeopleViewModel>(service);
            var vm = serviceAggregator.AggregateOperation(viewModelMapper.Map).Result;

            vm.GendersByAge.Count.Should().Be(3, "There should be a record for each age group (3)"); 
            vm.GendersByAge.FirstOrDefault(v => v.Key == 22).Value.Genders.Count.Should().Be(2, "There should be a record per gender (2)");
        }
    }
}
