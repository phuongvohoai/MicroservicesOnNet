using WAL.EventBus.Abstract;
using WAL.EventBus.Models;

namespace WAL.EventBus.Handler
{
    /// <summary>
    /// Define event bus handler base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventBusHandlerBase<T> where T : EventBase
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Handle(T message);
    }

    /// <summary>
    /// Define event bus handler base
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IEventBusHandlerBase<TRequest, TResponse>
        where TRequest : EventBusRequestBase
        where TResponse : EventBusResponseBase
    {
        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse SendRequest(TRequest request);
    }
}