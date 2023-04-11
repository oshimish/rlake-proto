using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenAI.GPT3;
using OpenAI.GPT3.Extensions;
using RlakeFunctionApp.Logging;
using RlakeFunctionApp.Mapper;
using RlakeFunctionApp.Repositories;
using RlakeFunctionApp.Settings;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(Startup))]

namespace RlakeFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var Configuration = new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

            services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"));
            services.Configure<ChatOptions>(Configuration.GetSection("Chat"));
            services.Configure<OpenAiOptions>(Configuration.GetSection("Chat:OpenAi"));

            var cosmosDbSettings = new CosmosDbSettings();
            Configuration.Bind(CosmosDbSettings.CosmosDbSectionKey, cosmosDbSettings);
            builder.Services.AddSingleton(cosmosDbSettings);

            //builder.Services.AddDbContext<ApiDbContext>(options =>
            //{
            //    options.UseCosmos(cosmosDbSettings.ConnectionString, cosmosDbSettings.DatabaseId);
            //});

            builder.Services.AddSingleton<IOpenApiConfigurationOptions, OpenApiConfigurationOptions>();

            services.AddAutoMapper(typeof(DtoMapperProfile));
            services.AddTransient<IChatService, ChatService>();
            services.AddOpenAIService();

            builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            builder.Services
                .AddHttpClient<INotificationService, NotificationService>(client =>
                {
                    client.BaseAddress = new Uri(Configuration.GetValue<string>("NotificationApiUrl"));
                });

            if (Debugger.IsAttached)
            {
                services.Configure<TelemetryConfiguration>(x => x.DisableTelemetry = true);
            }
        }

 
        public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
        {
            public OpenApiConfigurationOptions()
            {
                OpenApiVersion = OpenApiVersionType.V3;
            }
        }
    }

}
