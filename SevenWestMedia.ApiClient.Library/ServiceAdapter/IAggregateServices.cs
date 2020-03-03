using System;
using System.Threading.Tasks;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library.ServiceAdapter
{
    public interface IAggregateServices<out T, TV>
        where T : IModel, new()
    {
        Task<TV> AggregateOperation(Func<TV, T, TV> accumulator);
    }
}