using Katmanli.Core.Interfaces.DataAccessInterfaces;
using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Katmanli.Service.Mapping;
using Katmanli.Service.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using StackExchange.Redis;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Extensions.Hosting;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(Katmanli.Core.SharedLibrary.Logging.ConfigureLogging);

// Serilog yapýlandýrmasý
//var loggerConfiguration = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .Enrich.FromLogContext()
//    .Enrich.WithExceptionDetails()
//    .WriteTo.File("C:\\Users\\yavuz\\OneDrive\\Desktop\\VakifbankStaj\\Kütüphane Uygulamasý\\Katmanli.API\\wwwroot\\Logs\\logLibrary.txt");

//Log.Logger = loggerConfiguration.CreateLogger();



//Zipkin yapýlandýrmasý
builder.Services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("LibraryApi"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddZipkinExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
        });
});

builder.Services.AddSingleton<TracerProvider>(sp =>
{
    return Sdk.CreateTracerProviderBuilder()
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("LibraryApiTrace"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddZipkinExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
        })
        .Build();
});


// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(MapProfile));

//Servis Kayýtlarý

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IRedisServer, RedisServer>();
builder.Services.AddTransient<ParameterList>();
builder.Services.AddScoped<IMailServer , MailServer>();
builder.Services.AddScoped<ITokenCreator, TokenCreator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddTransient<DatabaseExecutions>();

//builder.Services.AddHostedService<KafkaConsumerService>();
//builder.Services.AddSingleton<KafkaConsumerService>();


//CORS Hatasý çözümü
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

//DbContext Ekleme
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


// Register ConnectionMultiplexer service
builder.Services.AddSingleton<ConnectionMultiplexer>(provider =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379");
    configuration.AbortOnConnectFail = false; 
    return ConnectionMultiplexer.Connect(configuration);
});

//SignalR
builder.Services.AddSignalR();


var app = builder.Build();

// KafkaConsumerService'i baþlat
//var kafkaConsumerService = app.Services.GetRequiredService<KafkaConsumerService>();
//kafkaConsumerService.StartListening();


// Middleware eklenir
app.UseRequestLoggingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseRouting();

app.MapHub<MainHub>("/connectServerHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



//Redis senkronizasyonunu baþlat
//var redisServer = app.Services.GetRequiredService<IRedisServer>();
//var redisInterval = TimeSpan.FromMinutes(5); // Senkronizasyon aralýðý
//redisServer.StartSyncScheduler(redisInterval);

//redisServer.SubscribeToKafkaTopic("192.168.1.110.dbo.UploadImages");

//Task.Run(() =>
//{
//    redisServer.SubscribeToKafkaTopic("192.168.1.110.dbo.UploadImages");
//});


app.Run();

