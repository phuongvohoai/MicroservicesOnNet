using EasyNetQ;
using WAL.EventBus.Configurations;

namespace WAL.EventBus.RabbitMQ
{
    /// <summary>
    /// Define rabbitMQ adapter
    /// </summary>
    internal class RabbitMqAdapter
    {
        /// <summary>
        /// The bus
        /// </summary>
        internal IBus Bus { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqAdapter"/> class.
        /// </summary>
        internal RabbitMqAdapter(IBusConfiguration configuration)
        {
            this.InitializeBus(configuration.GetConnectionString());
        }

        private void InitializeBus(string connection)
        {
            this.Bus = RabbitHutch.CreateBus(connection);
        }

        ~RabbitMqAdapter()
        {
            this.Bus.Dispose();
        }
    }
}