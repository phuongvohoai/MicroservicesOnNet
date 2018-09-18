namespace WAL.UserActivityLogs.API.Entities
{
    public class ActivityLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Activity { get; set; }

        public string Detail { get; set; }
        
        public string LogDate { get; set; }
    }
}
