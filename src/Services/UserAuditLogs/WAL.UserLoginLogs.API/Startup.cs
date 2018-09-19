namespace WAL.UserActivityLogs.API
{
    using Autofac;
    using EventBus.Abstract;
    using Infrastructure;
    using IntegrationEventHandlers;
    using IntegrationEvents;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using StartupBase = ServiceHost.StartupBase;

    public class Startup : StartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnConfigureContainer(ContainerBuilder builder)
        {
            base.OnConfigureContainer(builder);
            builder.RegisterType<DataContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ActivityLogService>().As<IActivityLogService>().InstancePerRequest().InstancePerDependency();
            builder.RegisterType<AddActivityLogEventHandler>().AsSelf().InstancePerDependency();

        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);
            services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(builder =>
            {
                builder.UseNpgsql(@"Host=postgre.data;Database=UserActivityLogsAPI;Username=root;Password=root");
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IPubSub>();
            var activityLogEventHandler = app.ApplicationServices.GetRequiredService<AddActivityLogEventHandler>();
            eventBus.SubcribeAsync(activityLogEventHandler);
        }

        public override void OnConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.OnConfigure(app, env, loggerFactory);
            this.ConfigureEventBus(app);
        }
    }
}
