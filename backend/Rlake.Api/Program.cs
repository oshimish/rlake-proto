using Microsoft.Extensions.Logging.ApplicationInsights;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Rlake.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
                config.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
            })
            .ConfigureLogging((context, builder) =>
            {
                var conString = context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] ?? context.Configuration["ApplicationInsights:ConnectionString"];

                if (!string.IsNullOrEmpty(conString))
                {
                    // Providing a connection string is required if you're using the
                    // standalone Microsoft.Extensions.Logging.ApplicationInsights package,
                    // or when you need to capture logs during application startup, such as
                    // in Program.cs or Startup.cs itself.
                    builder.AddApplicationInsights(
                        configureTelemetryConfiguration: (config) => config.ConnectionString = conString,
                        configureApplicationInsightsLoggerOptions: (options) => { }
                    );

                    // Capture all log-level entries from Program
                    builder.AddFilter<ApplicationInsightsLoggerProvider>(
                        typeof(Program).FullName, LogLevel.Debug);
                }

                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}