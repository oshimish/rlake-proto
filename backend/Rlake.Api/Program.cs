
using Microsoft.Extensions.Configuration;

namespace Rlake.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;
            services.Configure<AzureOptions>(configuration.GetSection("AzureStorage"));
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
            services.Configure<RabbitMqOptions>(configuration);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddProblemDetails();
            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddTransient<UploadFileService>();




            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}