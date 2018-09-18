namespace WAL.UserActivityLogs.API.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public interface IActivityLogService
    {
        IEnumerable<ActivityLog> GetAll();
        IEnumerable<ActivityLog> GetByUserId(int userId);
        ActivityLog Create(ActivityLog log);
    }

    public class ActivityLogService : IActivityLogService
    {
        private readonly DataContext dataContext;

        public ActivityLogService(DataContext dbContext)
        {
            this.dataContext = dbContext;
        }

        public IEnumerable<ActivityLog> GetAll()
        {
            return this.dataContext.ActivityLogs;
        }

        public IEnumerable<ActivityLog> GetByUserId(int userId)
        {
            return this.dataContext.ActivityLogs.Where(log => log.UserId == userId);
        }

        public ActivityLog Create(ActivityLog log)
        {
            return this.dataContext.ActivityLogs.Add(log).Entity;
        }
    }
}
