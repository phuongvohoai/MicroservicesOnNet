namespace WAL.ServiceHost
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using EventBus.Configurations;
    using EventBus.RabbitMQ;
    using Helpers;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class StartupBase
    {
        public StartupBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(options =>
            {
                // Add json output format
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }, ArrayPool<char>.Shared));
            });
            services.AddAutofac();
            this.OnConfigureServices(services);
        }

        /// <summary>
        /// Called when [configure services].
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void OnConfigureServices(IServiceCollection services)
        {

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
            this.OnConfigureContainer(builder);
            var configuration = new RabbitMQBusConfiguration();
            configuration = configuration.WithUser("rabbitmq").WithPassWord("rabbitmq")
                .WithHost("RabbitMQ") as RabbitMQBusConfiguration;
            builder.RegisterInstance(configuration).As<IBusConfiguration>().SingleInstance();
        }

        /// <summary>
        /// Called when [configure container].
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void OnConfigureContainer(ContainerBuilder builder)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    // the IsTrusted() extension method doesn't exist and
                    // you should implement your own as you may want to interpret it differently
                    // i.e. based on the current principal

                    var problemDetails = new ProblemDetails
                    {
                        Instance = $"urn:error:{Guid.NewGuid()}"
                    };

                    if (exception is BadHttpRequestException badHttpRequestException)
                    {
                        problemDetails.Title = "Invalid request";
                        problemDetails.Status = (int)typeof(BadHttpRequestException).GetProperty("StatusCode",
                            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(badHttpRequestException);
                        problemDetails.Detail = badHttpRequestException.Message;
                    }
                    else
                    {
                        problemDetails.Title = "An unexpected error occurred!";
                        problemDetails.Status = 500;
                        problemDetails.Detail = exception.Demystify().ToString();
                    }

                    // log the exception etc..

                    context.Response.StatusCode = problemDetails.Status.Value;
                    context.Response.WriteJson(problemDetails, "application/problem+json");
                });
            });
            this.OnConfigure(app, env, loggerFactory);
        }

        public virtual void OnConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

        }
    }
}
