namespace WAL.UserActivityLogs.API
{
    using Autofac;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ServiceHost;

    public class Startup : StartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnConfigureContainer(ContainerBuilder builder)
        {
            base.OnConfigureContainer(builder);
            builder.RegisterType<DataContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ActivityLogService>().As<IActivityLogService>().InstancePerRequest();
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);
            services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(builder =>
            {
                builder.UseNpgsql(@"Host=postgre.data;Database=UserActivityLogsAPI;Username=root;Password=root");
            });
        }
    }
}
