namespace WAL.UserActivityLogs.API
{
    using Autofac;
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
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);
        }
    }
}
