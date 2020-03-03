using SevenWestMedia.ApiClient.Console.ViewModels;
using SevenWestMedia.ApiClient.Library.Models;
using SevenWestMedia.ApiClient.Library.ServiceAdapter;
using Microsoft.Extensions.DependencyInjection;
    using System;

namespace SevenWestMedia.ApiClient.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var serviceProvider = startup.ConfigureServices();
            startup.Configure();

            var aggregator = serviceProvider.GetService<IAggregateServices<Person, PeopleViewModel>>();
            var viewModelMapper = serviceProvider.GetService<IMapper<Person, PeopleViewModel>>();
            
            var viewModel = aggregator.AggregateOperation(viewModelMapper.Map);
            System.Console.WriteLine(viewModel);
            System.Console.ReadLine();
        }
    }
}
