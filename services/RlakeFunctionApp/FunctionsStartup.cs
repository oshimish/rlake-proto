using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rlake.Api.Data;
using System;
using System.Configuration;

[assembly: FunctionsStartup(typeof(functions_csharp_entityframeworkcore.Startup))]

namespace functions_csharp_entityframeworkcore
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("CosmosConnectionString");

            builder.Services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseCosmos(SqlConnection, "RlakeDB");
            });

            builder.Services.AddSingleton<IOpenApiConfigurationOptions, OpenApiConfigurationOptions>();
        }

        public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
        {
            public OpenApiConfigurationOptions()
            {
                this.OpenApiVersion = OpenApiVersionType.V3;
            }
        }
    }

}
