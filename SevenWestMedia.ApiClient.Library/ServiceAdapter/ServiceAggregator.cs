using System;
using System.Linq;
using System.Threading.Tasks;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library.ServiceAdapter
{
    public class ServiceAggregator<T, TV> : IAggregateServices<T, TV>
        where T : IModel, new() where TV : new()
    {
        private readonly ISampleTestService _service;

        public ServiceAggregator(ISampleTestService service)
        {
            _service = service;
        }

        public async Task<TV> AggregateOperation(Func<TV,T, TV> accumulator)
        {
            var records = await _service.DeserialiseStream<T>();
            return records.Aggregate<T,TV>(new TV(), accumulator);
        }
    }
}