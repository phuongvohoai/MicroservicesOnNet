using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using WAL.Identity.API.Entities;

namespace WAL.Identity.API
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using AutoMapper;
    using EventBus.Configurations;
    using EventBus.RabbitMQ;
    using Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Services;
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
            services.AddCors();
            services.AddMvc(options =>
            {
                // Add json output format
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }, ArrayPool<char>.Shared));
            });
            services.AddAutoMapper();

            var mysqlConnectionString = this.Configuration.GetConnectionString("MySqlConnection");
            services.AddEntityFrameworkMySql().AddDbContext<DataContext>(builder =>
            {
                builder.UseMySql(mysqlConnectionString);
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            //            var userId = int.Parse(context.Principal.Identity.Name);
            //            var user = userService.GetById(userId);
            //            if (user == null)
            //            {
            //                // return unauthorized if user no longer exists
            //                context.Fail("Unauthorized");
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<User>()
                .AddConfigurationStore(option =>
                {
                    option.ConfigureDbContext = builder => builder.UseMySql(mysqlConnectionString,
                        mySqlOptionsAction: mysqlOptions =>
                        {
                            mysqlOptions.MigrationsAssembly(migrationsAssembly);
                            mysqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                        });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseMySql(mysqlConnectionString,
                        mySqlOptionsAction: mysqlOptions =>
                        {
                            mysqlOptions.MigrationsAssembly(migrationsAssembly);
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            mysqlOptions.EnableRetryOnFailure(maxRetryCount: 15,
                                maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                }).Services.AddTransient<IProfileService, ProfileService>();
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

            builder.RegisterType<DataContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest().InstancePerDependency();
            var configuration = new RabbitMQBusConfiguration();
            configuration = configuration.WithUser("rabbitmq").WithPassWord("rabbitmq")
                .WithHost("RabbitMQ") as RabbitMQBusConfiguration;
            builder.RegisterInstance(configuration).As<IBusConfiguration>().SingleInstance();
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
            app.UseIdentityServer();
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
        }
    }
}
