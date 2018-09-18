namespace WAL.Identity.API
{
    using System.IO;
    using Helpers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     A factory for creating derived <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> instances. Implement this interface to enable
    ///     design-time services for context types that do not have a public default constructor. At design-time,
    ///     derived <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> instances can be created in order to enable specific design-time
    ///     experiences such as Migrations. Design-time services will automatically discover implementations of
    ///     this interface that are in the startup assembly or the same assembly as the derived context.
    /// </summary>
    /// <typeparam name="DataContext">The type of the context.</typeparam>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="args"> Arguments provided by the design-time service. </param>
        /// <returns> An instance of <typeparamref name="DataContext" />. </returns>
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("MigrationContextConnection");
            builder.UseMySql(connectionString);
            return new DataContext(builder.Options);
        }
    }
}
