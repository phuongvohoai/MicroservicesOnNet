namespace WAL.IntegrationEvents
{
    using EventBus.Abstract;

    public class ActivityLogAddingEvent : EventBase
    {
        public int UserId { get; set; }

        public string Activity { get; set; }

        public string Detail { get; set; }

        public string LogDate { get; set; }
    }
}
