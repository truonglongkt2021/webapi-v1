using Autofac.Extensions.DependencyInjection;
using Invedia.Core.ConfigUtils;
using Invedia.Core.Constants;
using Invedia.Core.EnvUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;
using StreamFile.Contract.Service;
using StreamFile.Core.Configs;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace StreamFile.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigurationFileName.AppSettings, false, true)
                .AddJsonFile($"appsettings.{EnvHelper.CurrentEnvironment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            try
            {
                ConsoleConfig();
                var host = CreateHostBuilder(args, config).Build();
                OnAppStart(host);
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                // Log.Logger will likely be internal type "Serilog.Core.Pipeline.SilentLogger".
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    // Loading configuration or Serilog failed.
                    // This will create a logger that can be captured by Azure logger.
                    // To enable Azure logger, in Azure Portal:
                    // 1. Go to WebApp
                    // 2. App Service logs
                    // 3. Enable "Application Logging (Filesystem)", "Application Logging (Filesystem)" and "Detailed error messages"
                    // 4. Set Retention Period (Days) to 10 or similar value
                    // 5. Save settings
                    // 6. Under Overview, restart web app
                    // 7. Go to Log Stream and observe the logs
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                }

                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot config)
        {
            var webHost = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());

            webHost = webHost.ConfigureAppConfiguration((context, cnf) =>
            {
                var root = cnf.Build();
                var useVault = root.GetValue("UseVault", false);
                if (useVault)
                {
                    cnf.AddAzureKeyVault(
                        $"https://{root["KeyVault:Vault"]}.vault.azure.net/",
                        root["KeyVault:ClientId"],
                        root["KeyVault:ClientSecret"]);
                }
                root = cnf.Build();

                SystemSettingModel.Configs = root;
                SystemSettingModel.Instance = root.Get<SystemSettingModel>();
                MomoSettingModel.Instance = root.GetSection<MomoSettingModel>("MomoSetting");
                //CryptoSettingModel.Instance = root.GetSection<CryptoSettingModel>("CryptoSetting");
                //PortalSettingModel.Instance = root.GetSection<PortalSettingModel>("PortalSetting");
            });

            webHost = webHost
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .CaptureStartupErrors(true)
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration)
                                .Enrich.FromLogContext();
                        });
                });

            return webHost;
        }

        public static void ConsoleConfig()
        {
            var welcome =
                $@"Welcome {EnvHelper.MachineName}, {PlatformServices.Default.Application.ApplicationName} v{PlatformServices.Default.Application.ApplicationVersion} - {EnvHelper.CurrentEnvironment} | {
                        PlatformServices.Default.Application.RuntimeFramework.FullName
                    } | {RuntimeInformation.OSDescription}";

            Console.Title = welcome;

            Console.WriteLine(welcome);
        }

        public static void OnAppStart(IHost webHost)
        {
            using var scoped = webHost.Services.CreateScope();
            var bootstrapperService = scoped.ServiceProvider.GetRequiredService<IBootstrapperService>();
            bootstrapperService.InitialAsync().Wait();
        }
    }
}