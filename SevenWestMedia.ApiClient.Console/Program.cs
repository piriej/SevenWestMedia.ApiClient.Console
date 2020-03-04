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
            PeopleViewModel viewModel = new PeopleViewModel();

            try
            {
                // For a scenario where the console does more than one task, This would be best execute as an observable
                // for this simple scenario the thread result is used.
                viewModel = aggregator.AggregateOperation(viewModelMapper.Map).Result;
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("A policy has been breached, please try again later.");
                System.Console.ResetColor();
            }

            System.Console.WriteLine();
            System.Console.BackgroundColor = ConsoleColor.Blue;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(viewModel);
            System.Console.ResetColor();
            System.Console.ReadLine();
        }
    }
}
