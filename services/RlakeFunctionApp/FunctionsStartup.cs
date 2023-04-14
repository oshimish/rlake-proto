using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using AzureAppConfigSampleFunction;
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
    public class Startup : AzureAppConfigStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var Configuration = builder.GetContext().Configuration;

            services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"));
            services.Configure<ChatOptions>(Configuration.GetSection("Chat"));
            services.Configure<OpenAiOptions>(Configuration.GetSection("Chat:OpenAi"));

            var cosmosDbSettings = new CosmosDbSettings();
            Configuration.Bind(CosmosDbSettings.CosmosDbSectionKey, cosmosDbSettings);
            builder.Services.AddSingleton(cosmosDbSettings);

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
                services.PostConfigure<TelemetryConfiguration>(x => x.DisableTelemetry = true);
            }
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultUrl = builtConfig["KeyVaultUrl"];
            if (!string.IsNullOrEmpty(keyVaultUrl))
            {
                //var credential = new DefaultAzureCredential();
                //builder.ConfigurationBuilder.AddAzureKeyVault(new Uri(keyVaultUrl), credential);
            }

            base.ConfigureAppConfiguration(builder);

            //var Configuration = new ConfigurationBuilder()
            //                 .AddConfiguration(builder.GetContext().Configuration)
            //                 .SetBasePath(Environment.CurrentDirectory)
            //                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            //                 .AddEnvironmentVariables()
            //                 .Build();
            
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
