using System;
using System.Threading.Tasks;
using WAL.EventBus.Models;

namespace WAL.EventBus.Abstract
{
    /// <summary>
    /// Define bus reponse interface
    /// </summary>
    public interface IBusResponse
    {
        /// <summary>
        /// Responds the specified responder.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="responder">The responder.</param>
        /// <returns></returns>
        IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
            where TRequest : EventBusRequestBase
            where TResponse : EventBusResponseBase;

        /// <summary>
        /// Responds the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="responder">The responder.</param>
        /// <returns></returns>
        IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
            where TRequest : EventBusRequestBase
            where TResponse : EventBusResponseBase;
    }
}