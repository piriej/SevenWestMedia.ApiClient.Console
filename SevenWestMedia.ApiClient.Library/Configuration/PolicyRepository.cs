using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace SevenWestMedia.ApiClient.Library.Configuration
{
    public class PolicyRepository
    {
        /// <summary>
        /// On 5XX / 408 / 404 wait and retry 4 times with an asymptotic delay.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// After 5 handled events break for 30 seconds.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }

   
}