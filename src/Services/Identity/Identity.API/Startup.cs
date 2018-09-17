using Autofac;
using WAL.EventBus.Configurations;
using WAL.EventBus.RabbitMQ;

namespace Identity.API
{
    using Entity;
    using Infrastructure.Repositories;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddEntityFrameworkMySql().AddDbContext<ApplicationUserContext>(builder =>
            {
                builder.UseMySql(this.Configuration.GetConnectionString("MySqlConnection"));
            });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new EventBusModule());

            builder.RegisterType<ApplicationUserContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<LoginService>().As<ILoginService<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserRepository>().InstancePerRequest();
            var configuration = new RabbitMQBusConfiguration();
            configuration = configuration.WithUser("rabbitmq").WithPassWord("rabbitmq")
                .WithHost("RabbitMQ") as RabbitMQBusConfiguration;
            builder.RegisterInstance(configuration).As<IBusConfiguration>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
