namespace WAL.UserActivityLogs.API.Infrastructure
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}
