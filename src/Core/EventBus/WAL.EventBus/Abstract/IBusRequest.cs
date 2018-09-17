using System.Threading.Tasks;
using WAL.EventBus.Models;

namespace WAL.EventBus.Abstract
{
    public interface IBusRequest
    {
        TResponse Request<TRequest, TResponse>(TRequest request)
            where TRequest : EventBusRequestBase
            where TResponse : EventBusResponseBase;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : EventBusRequestBase
            where TResponse : EventBusResponseBase;
    }
}