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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);




// Swagger servisi ve swagger json üretimi için.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp 99 API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                                }
                            },
                            new string[] {}
                }});

});

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


//Jwt yapýlandýrýlmasý
builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    string secret = builder.Configuration.GetValue<string>("AppSettings:SecretKey");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" // Rol doðrulamasý için
                    };
                });

//Sadece Adminin kullanabilmesi için API'leri için authorization iþlemi yapalým.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "1"));
});



//builder.Services.AddMemoryCache();
//builder.Services.AddResponseCaching();


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



// Reflection Test
//Reflecton reflecton = new Reflecton();
//var rtpe = reflecton.GetType();
//var properties = rtpe.GetProperties();
//var metods = rtpe.GetMethods();
//var instance = Activator.CreateInstance(rtpe);
//properties.ToList().ForEach(property => Console.WriteLine(property));
//metods.ToList().ForEach(metod => Console.WriteLine(metod));


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

