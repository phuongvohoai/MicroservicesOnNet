namespace WAL.UserActivityLogs.API.IntegrationEventHandlers
{
    using AutoMapper;
    using Entities;
    using EventBus.Handler;
    using Infrastructure;
    using IntegrationEvents;

    public class AddActivityLogEventHandler : IEventBusHandlerBase<ActivityLogAddingEvent>
    {
        /// <summary>
        /// The activity log service
        /// </summary>
        private readonly IActivityLogService activityLogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddActivityLogEventHandler"/> class.
        /// </summary>
        /// <param name="activityLogService">The activity log service.</param>
        public AddActivityLogEventHandler(IActivityLogService activityLogService)
        {
            this.activityLogService = activityLogService;
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(ActivityLogAddingEvent message)
        {
            var activityLog = Mapper.Map<ActivityLog>(message);
            this.activityLogService.Create(activityLog);
        }
    }
}
