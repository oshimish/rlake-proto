using Microsoft.ApplicationInsights.Extensibility;
using OpenAI.GPT3.Extensions;
using Rlake.Api.Mapper;
using System.Diagnostics;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Rlake.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"));
        services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMq"));
        services.Configure<AppOptions>(Configuration);
        services.Configure<ChatOptions>(Configuration.GetSection("Chat"));
        services.Configure<OpenAiOptions>(Configuration.GetSection("Chat:OpenAi"));

        var options = Configuration.Get<AppOptions>();

        var connectionString = Configuration.GetConnectionString("ServiceBusConnection")!;
        services.AddSingleton(new AzureServiceBusClient(connectionString));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var docgenAssembly = this.GetType().Assembly;
                
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{docgenAssembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
                
            options.EnableAnnotations();
        });
        services.AddProblemDetails();
        services.AddApplicationInsightsTelemetry();
        services.AddCors();
        services.AddAutoMapper(typeof(DtoMapperProfile));

        services.AddTransient<UploadFileService>();
        services.AddTransient<ChatService>();

        services.AddDbContext<ApiDbContext>(options =>
        {
            options.UseCosmos(Configuration.GetConnectionString("Cosmos")!, "RlakeDB");
            //options.UseInMemoryDatabase("default");
        });

        services.AddAntiforgery();

        services.AddOpenAIService();

        if (Environment.IsDevelopment() && Debugger.IsAttached)
        {
            // any custom configuration can be done here:
            services.Configure<TelemetryConfiguration>(x => x.DisableTelemetry = true);
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        if (!Environment.IsProduction())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(builder => builder            
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           );

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("API");
            });
        });
    }
}
