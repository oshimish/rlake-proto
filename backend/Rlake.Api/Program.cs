
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenAI.GPT3.Extensions;
using Rlake.Api.Options;

namespace Rlake.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var Configuration = builder.Configuration;
            services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"));
            services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMq"));
            services.Configure<AppOptions>(Configuration);
            services.Configure<ChatOptions>(Configuration.GetSection("Chat"));
            services.Configure<OpenAiOptions>(Configuration.GetSection("Chat:OpenAi"));

            var options = Configuration.Get<AppOptions>();

            var connectionString = Configuration.GetConnectionString("ServiceBusConnection")!;
            services.AddSingleton<AzureServiceBusClient>(new AzureServiceBusClient(connectionString));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddProblemDetails();
            builder.Services.AddApplicationInsightsTelemetry();
            services.AddCors();


            services.AddTransient<UploadFileService>();
            services.AddTransient<ChatService>();

            services.AddDbContext<ApiDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("Default"));
                options.UseInMemoryDatabase("default");
            });

            services.AddOpenAIService();

            var app = builder.Build();

            if (!app.Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}