using System;

namespace WAL.EventBus.Models
{
    /// <summary>
    /// Define event bus request base
    /// </summary>
    public class EventBusRequestBase
    {
        /// <summary>
        /// Gets or sets the request dated.
        /// </summary>
        /// <value>
        /// The request dated.
        /// </value>
        public DateTime RequestDated { get; set; }
    }
}