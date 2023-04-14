using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

//[assembly: FunctionsStartup(typeof(AzureAppConfigSampleFunction.AzureAppConfigStartup))]

namespace AzureAppConfigSampleFunction
{
    public class AzureAppConfigStartup : FunctionsStartup
    {
        public const string AppKey = "HttpApiApp";
        public const string SentinelKey = $"{AppKey}:Settings:Sentinel"; //todo
        public const string KeysSelector = $"*";

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var userSecretConfig = new ConfigurationBuilder();
            userSecretConfig.AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), true);
            var azAppConfigConnection = userSecretConfig.Build()["AppConfig"];

            if (string.IsNullOrEmpty(azAppConfigConnection))
            {
                throw new InvalidOperationException("AppConfig conection string not set ");
            }

            // Use the connection string if it is available.
            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(azAppConfigConnection);
                // Load all keys that start with 'TestApp:' and have no label
                options.Select(KeysSelector);
                options.TrimKeyPrefix($"{AppKey}:");

                options.ConfigureKeyVault(options => {
                    var credential = new DefaultAzureCredential();
                    options.SetCredential(credential);
                });

                // Configure to reload configuration if the registered key 'TestApp:Settings:Sentinel' is modified.
                // Use the default cache expiration of 30 seconds. It can be overriden via AzureAppConfigurationRefreshOptions.SetCacheExpiration.
                options.ConfigureRefresh(refresh =>
                {
                    refresh.Register(SentinelKey, refreshAll: true);
                });
                // Load all feature flags with no label. To load specific feature flags and labels, set via FeatureFlagOptions.Select.
                // Use the default cache expiration of 30 seconds. It can be overriden via FeatureFlagOptions.CacheExpirationInterval.
                options.UseFeatureFlags();
            });
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Make Azure App Configuration services and feature manager available through dependency injection
            builder.Services.AddAzureAppConfiguration();
            builder.Services.AddFeatureManagement();
        }
    }
}
