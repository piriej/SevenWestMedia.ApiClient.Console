using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Dsl;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library.Test.Conventions
{
    public class HttpClientFixtureBuilder<T> : IBuildFixtures<T>
        where T : IModel
    {
        private List<HttpStatusCode> _statusCode = new List<HttpStatusCode>();
        public List<T> HttpBodyModel { get; private set; }
        private readonly Fixture _fixture;
        private HttpClient _httpClient;
        public Mock<HttpMessageHandler> HandlerMock { get; private set; }

        public HttpClientFixtureBuilder()
        {
            _fixture = new Fixture();
        }

        public HttpClient Build()
        {
            if (_httpClient == null)
                throw new Exception("HttpClient Could not be created, please call WithGet() first.");

            return _httpClient;
        }

        public HttpClientFixtureBuilder<T> WithStatusCode(HttpStatusCode statusCode)
        {
            _statusCode.Add(statusCode);
            return this;
        }

        public HttpClientFixtureBuilder<T> WithStatusCode(List<HttpStatusCode> statusCodes)
        {
            _statusCode = statusCodes;
            return this;
        }


        public HttpClientFixtureBuilder<T> WithModel(List<T> models)
        {
            HttpBodyModel = models;
            return this;
        }

        public HttpClientFixtureBuilder<T> WithModel(
            Expression<Func<ICustomizationComposer<T>, IPostprocessComposer<T>>> expression, int count = 1)
        {
            var customizationComposer = _fixture
                .Build<T>();

            var func = expression.Compile();
            var composer = func(customizationComposer);

            var model = composer.Create();

            var models = new List<T>();
            if (count > 1)
            {
                _fixture.RepeatCount = count - 1;
                models = _fixture.Create<List<T>>();
            }

            models.Add(model);

            HttpBodyModel = models;
            return this;
        }

        public HttpClientFixtureBuilder<T> WithAllModel(
            Expression<Func<ICustomizationComposer<T>, IPostprocessComposer<T>>> expression, int count = 1)
        {
            _fixture.Register(() =>
            {
                var customizationComposer = _fixture
                    .Build<T>();

                var func = expression.Compile();
                var composer = func(customizationComposer);

                return composer.Create();
            });

            HttpBodyModel = _fixture.CreateMany<T>().ToList();

            return this;
        }

        public HttpClientFixtureBuilder<T> WithGet()
        {
            WithGet(_statusCode, HttpBodyModel);
            return this;
        }

        public HttpClientFixtureBuilder<T> WithGet(List<HttpStatusCode> statusCode, List<T> models)
        {
            if (models == null)
                throw new Exception("Model must be defined prior to call to WithGet");

            var json = JsonConvert.SerializeObject(models);

            var queue = new Queue<HttpStatusCode>(statusCode);

            HandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            HandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = queue.SafeDequeue(),
                    Content = new StringContent(json),
                })
                .Verifiable();

            _httpClient = new HttpClient(HandlerMock.Object)
            {
                BaseAddress = new Uri("http://fakeTestUri.com/"),
            };

            return this;
        }
    }

    public static class QueueExtensions
    {
        public static HttpStatusCode SafeDequeue(this Queue<HttpStatusCode> queue)
        {
            if (queue.Count == 1)
                return queue.Peek();
            return queue.Dequeue();
        }
    }
}