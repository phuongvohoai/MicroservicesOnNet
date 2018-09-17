using System;
using System.Threading.Tasks;
using WAL.EventBus.Abstract;
using WAL.EventBus.Configurations;
using WAL.EventBus.Handler;
using WAL.EventBus.Models;

namespace WAL.EventBus.RabbitMQ
{
    /// <summary>
    /// Define event bus
    /// </summary>
    /// <seealso cref="WAL.EventBus.Abstract.IPubSub" />
    /// <seealso cref="WAL.EventBus.Abstract.IBusRequest" />
    /// <seealso cref="WAL.EventBus.Abstract.IBusResponse" />
    public class EventBus : IPubSub, IBusRequest, IBusResponse
    {
        /// <summary>
        /// Gets the adapter.
        /// </summary>
        /// <value>
        /// The adapter.
        /// </value>
        private readonly RabbitMqAdapter adapter;

        public EventBus(IBusConfiguration configuration)
        {
            this.adapter = new RabbitMqAdapter(configuration);
        }

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        public void Publish<T>(T message) where T : EventBase
        {
            this.adapter.Bus.Publish(message);
        }

        /// <summary>
        /// Publishes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        public void PublishAsync<T>(T message) where T : EventBase
        {
            this.adapter.Bus.PublishAsync(message);
        }

        /// <summary>
        /// Subcribes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH">The type of the h.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="handler">The handler.</param>
        public void Subcribe<T, TH>(T message, TH handler) where T : EventBase where TH : IEventBusHandlerBase<T>
        {
            this.adapter.Bus.Subscribe<T>(string.Empty, handler.Handle);
        }

        /// <summary>
        /// Subcribes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH">The type of the h.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="handler">The handler.</param>
        public void SubcribeAsync<T, TH>(T message, TH handler) where T : EventBase where TH : IEventBusHandlerBase<T>
        {
            Task OnMessage(T request) => Task.Run(() => { handler.Handle(request); });
            this.adapter.Bus.SubscribeAsync<T>(string.Empty, OnMessage);
        }

        /// <summary>
        /// Requests the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Request<TRequest, TResponse>(TRequest request) where TRequest : EventBusRequestBase where TResponse : EventBusResponseBase
        {
            return this.adapter.Bus.Request<TRequest, TResponse>(request);
        }

        /// <summary>
        /// Requests the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : EventBusRequestBase where TResponse : EventBusResponseBase
        {
            return this.adapter.Bus.RequestAsync<TRequest, TResponse>(request);
        }

        /// <summary>
        /// Responds the specified responder.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="responder">The responder.</param>
        /// <returns></returns>
        public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder) where TRequest : EventBusRequestBase where TResponse : EventBusResponseBase
        {
            return this.adapter.Bus.Respond(responder);
        }

        /// <summary>
        /// Responds the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="responder">The responder.</param>
        /// <returns></returns>
        public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder) where TRequest : EventBusRequestBase where TResponse : EventBusResponseBase
        {
            return this.adapter.Bus.RespondAsync(responder);
        }
    }
}