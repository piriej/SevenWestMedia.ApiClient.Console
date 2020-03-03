using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SevenWestMedia.ApiClient.Library.Configuration;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library.ServiceAdapter
{
    public class SampleTestService : ISampleTestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _serviceEndpoint;
        private readonly int _requestTimeout;

        public SampleTestService(HttpClient httpClient, Config config)
        {
            _httpClient = httpClient;
            _serviceEndpoint = config.TestEndpoint;
            _requestTimeout = config.RequestTimeout;
        }

        public async Task<IEnumerable<T>> DeserialiseStream<T>()
            where T : IModel, new()
        {
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(_requestTimeout));
            using (var request = new HttpRequestMessage(HttpMethod.Get, _serviceEndpoint))
            using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserialiseStream<IEnumerable<T>>(stream);
            }

            return new List<T>();
        }

//        public IObservable<IEnumerable<T>> ToObservable<T>()
//            where T : IModel, new()
//        {
//            return DeserialiseStream<T>()
//                    .ToObservable()
//                    .SelectMany(results => results)
//                    .Select(x => x);
//        }

        private static T DeserialiseStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();
                var searchResult = jsonSerializer.Deserialize<T>(jsonTextReader);
                return searchResult;
            }
        }
    }
}
