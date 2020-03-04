using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Polly;
using Polly.CircuitBreaker;
using SevenWestMedia.ApiClient.Console.ViewModels;
using SevenWestMedia.ApiClient.Library.Configuration;
using SevenWestMedia.ApiClient.Library.Models;
using SevenWestMedia.ApiClient.Library.ServiceAdapter;
using SevenWestMedia.ApiClient.Library.Test.Conventions;
using Polly.Registry;

namespace SevenWestMedia.ApiClient.Library.Test
{
    public class PolicyTests
    {
        /// <summary>
        /// 2.	The endpoint could go down
        ///
        /// This could involve a server timeout
        /// See:https://github.com/dotnet/extensions/issues/851 there is acurrently a bug ConfigurePrimaryHttpMessageHandler<T>() not loading
        /// </summary>
//        public void AppIsResilientToServerTimeout(
//            IMapper<Person, PeopleViewModel> viewModelMapper,
//            IOptions<Config> config,
//            IPolicyRegistry<string> policyRegistry,
//            HttpClient httpClient,
//            IAggregateServices<Person, PeopleViewModel> serviceAggregator)
//        {
//            var mockPolicy = new Mock<IAsyncPolicy>();
//            mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task<HttpResponseMessage>>>()))
//                .Throws<BrokenCircuitException>();
//
//            policyRegistry["CircuitBreakerPolicy"] = mockPolicy.Object;
//
//            Func<PeopleViewModel> func = () => serviceAggregator.AggregateOperation(viewModelMapper.Map).Result;
//            func.Should().Throw<BrokenCircuitException>();
//
//        }
    }
}
