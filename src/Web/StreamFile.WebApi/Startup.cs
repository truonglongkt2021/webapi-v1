using Autofac;
using Flurl.Http;
using Flurl.Http.Configuration;
using Hangfire;
using Hangfire.SqlServer;
using Invedia.Core.ConfigUtils;
using Invedia.DI;
using Invedia.Web.HealthCheck;
using Invedia.Web.Middlewares.HttpContextMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using StreamFile.Core.Configs;
using StreamFile.Core.Constants;
using StreamFile.Core.Utils;
using StreamFile.WebApi.Filters;
using StreamFile.WebApi.Modules;
using System;
using Microsoft.AspNetCore.SignalR;
using StreamFile.WebApi.Extensions;
using StreamFile.Service.Job;

namespace StreamFile.WebApi
{
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
            // System Setting
            services.AddSystemSetting(Configuration.GetSection<SystemSettingModel>("SystemSetting"));
            services.AddInvediaHttpContext();

            services.AddApplicationInsightsTelemetry();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => //AllowOrigin
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("https://localhost:44336",
                                 "https://localhost:44347");
            }));
            
            services.AddMemoryCache();

            services.AddDataProtection();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StreamFile.WebApi", Version = "v1" });
            });

            // Mapper
            //services.AddInvediaAutoMapper();

            services.AddMvcApi();
            services.AddSignalRService();
            // Auto Register Dependency Injection
            services.AddDI();
            services.PrintServiceAddedToConsole();

            // Flurl Config
            FlurlHttp.Configure(config =>
            {
                config.JsonSerializer = new NewtonsoftJsonSerializer(Formattings.JsonSerializerSettings);
            });

            services.AddInvediaHealthCheck(configure => { configure.DbConnectionString = SystemHelper.AppDb; });
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(SystemHelper.AppDb,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));

            services.AddHangfireServer();
            services.AddSingleton<IDemoJob, DemoJob>();
            services.AddControllers().AddNewtonsoftJson();

            //services.AddDbContext<AppDbContext>(options =>
            //                    options.UseSqlServer(SystemHelper.AppDb));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy"); //"AllowOrigin


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //  app.UseHangfireDashboard();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                        new HangfireAuthorizationFilter
                        {
                            User = "hangfire",
                            Pass = "Login@2021az"
                        }
                    },

                // IsReadOnlyFunc = _ => true
            });

            var swagger = Configuration.GetValue("UseSwagger", false);
            if (swagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StreamFile.SSO v1"));
            }

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            //app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseRouting();
            app.UseSignalRService();
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "area", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // System Setting
            app.UseSystemSetting();
            app.UseInvediaHttpContext();
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<UserHub>("/hubs/doc");
            //});
            app.UseInvediaHealthCheck();
            var jobService = app.ApplicationServices.GetRequiredService<IDemoJob>();
            //var comJobService = app.ApplicationServices.GetRequiredService<IComJob>();
            //var calcComService = app.ApplicationServices.GetRequiredService<ICalcComJob>();
            RecurringJob.AddOrUpdate("Demo-push-signalr", () => jobService.Execute(), Cron.Minutely());
            //RecurringJob.AddOrUpdate("bet-summary-daily", () => jobService.Execute("daily"), Cron.Daily);
            //RecurringJob.AddOrUpdate("commission-hourly", () => comJobService.Execute("hourly"), Cron.Hourly(30));
            //RecurringJob.AddOrUpdate("commission-daily", () => comJobService.Execute("daily"), Cron.Daily());
            //RecurringJob.AddOrUpdate("calcommission-week", () => calcComService.Execute(), Cron.Weekly(DayOfWeek.Monday,1));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
    }
}