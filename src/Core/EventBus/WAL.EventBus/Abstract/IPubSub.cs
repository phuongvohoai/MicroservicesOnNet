﻿using System.Threading.Tasks;
using WAL.EventBus.Handler;

namespace WAL.EventBus.Abstract
{
    /// <summary>
    /// Define pubsub interface
    /// </summary>
    public interface IPubSub
    {
        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        void Publish<T>(T message) where T : EventBase;

        /// <summary>
        /// Publishes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        void PublishAsync<T>(T message) where T : EventBase;

        /// <summary>
        /// Subcribes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH">The type of the h.</typeparam>
        /// <param name="handler">The handler.</param>
        void Subcribe<T>(IEventBusHandlerBase<T> handler)
            where T : EventBase;

        /// <summary>
        /// Subcribes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH">The type of the h.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        void SubcribeAsync<T>(IEventBusHandlerBase<T> handler)
            where T : EventBase;
    }
}