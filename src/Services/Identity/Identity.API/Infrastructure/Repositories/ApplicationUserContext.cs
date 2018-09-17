namespace Identity.API.Infrastructure.Repositories
{
    using Entity;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationUserContext : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public ApplicationUserContext(DbContextOptions<ApplicationUserContext> options) : base(options)
        {

        }
    }
}
