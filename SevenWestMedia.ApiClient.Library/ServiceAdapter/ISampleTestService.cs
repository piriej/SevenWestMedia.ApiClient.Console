using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library.ServiceAdapter
{
    public interface ISampleTestService
    {
        Task<IEnumerable<T>> DeserialiseStream<T>() where T : IModel, new();

//        IObservable<T> ToObservable<T>()
//            where T : IEnumerable<IModel>, new();
    }
}