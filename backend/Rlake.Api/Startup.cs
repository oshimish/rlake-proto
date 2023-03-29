using OpenAI.GPT3.Extensions;
using Rlake.Api.Mapper;

namespace Rlake.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
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
        services.AddSingleton<AzureServiceBusClient>(new AzureServiceBusClient(connectionString));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddApplicationInsightsTelemetry();
        services.AddCors();
        services.AddAutoMapper(typeof(DtoMapperProfile));

        services.AddTransient<UploadFileService>();
        services.AddTransient<ChatService>();

        services.AddDbContext<ApiDbContext>(options =>
        {
            //options.UseSqlServer(Configuration.GetConnectionString("Default"));
            options.UseInMemoryDatabase("default");
        });

        services.AddOpenAIService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsProduction())
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
        });
    }
}
