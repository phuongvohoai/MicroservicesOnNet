using System;

namespace WAL.EventBus.Models
{
    /// <summary>
    /// Define event bus response base
    /// </summary>
    public class EventBusResponseBase
    {
        /// <summary>
        /// Gets or sets the response date.
        /// </summary>
        /// <value>
        /// The response date.
        /// </value>
        public DateTime ResponseDate { get; set; }
    }
}