namespace WAL.UserActivityLogs.API.ViewModel
{
    public class ActivityLogViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Activity { get; set; }

        public string Detail { get; set; }

        public string LogDate { get; set; }

        public string Username { get; set; }
    }
}
