using System;
using System.Collections.Generic;
using System.Text;

namespace WAL.EventBus.Configurations
{
    /// <summary>
    /// Define Ibus configuration
    /// </summary>
    public interface IBusConfiguration
    {
        /// <summary>
        /// Withes the host.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        IBusConfiguration WithHost(string hostName);

        /// <summary>
        /// Withes the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        IBusConfiguration WithPort(int port);

        /// <summary>
        /// Withes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        IBusConfiguration WithUser(string user);

        /// <summary>
        /// Withes the pass word.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        IBusConfiguration WithPassWord(string password);

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        string GetConnectionString();
    }
}
