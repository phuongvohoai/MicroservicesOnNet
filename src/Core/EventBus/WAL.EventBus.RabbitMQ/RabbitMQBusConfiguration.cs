using WAL.EventBus.Configurations;

namespace WAL.EventBus.RabbitMQ
{
    /// <summary>
    /// Define rabbitMQBusConfiguration
    /// </summary>
    /// <seealso cref="WAL.EventBus.Configurations.IBusConfiguration" />
    public class RabbitMQBusConfiguration : IBusConfiguration
    {
        /// <summary>
        /// The host name
        /// </summary>
        private string hostName = "localhost";

        /// <summary>
        /// The user
        /// </summary>
        private string user = "guest";

        /// <summary>
        /// The password
        /// </summary>
        private string password = "guest";

        /// <summary>
        /// The port
        /// </summary>
        private int port = -1;

        /// <summary>
        /// Withes the host.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        public IBusConfiguration WithHost(string hostName)
        {
            this.hostName = hostName;
            return this;
        }

        /// <summary>
        /// Withes the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public IBusConfiguration WithPort(int port)
        {
            this.port = port;
            return this;
        }

        /// <summary>
        /// Withes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public IBusConfiguration WithUser(string user)
        {
            this.user = user;
            return this;
        }

        /// <summary>
        /// Withes the pass word.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public IBusConfiguration WithPassWord(string password)
        {
            this.password = password;
            return this;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            var host = this.port != -1 ? $"{this.hostName}:{this.port}" : this.hostName;
            return $"host={host};username={this.user};password={this.password}";
        }
    }
}