using SignalRApplication.Hubs;
using Owin;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MainHub>();
builder.Services.AddScoped<MainHub,MainHub>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddSignalR();
var app = builder.Build();

app.UseCors("AllowAll");

app.MapHub<MainHub>("/connectHub");

app.Map("/connectHub", map =>
{
    map.UseCors("AllowAll");
}
);

app.MapGet("/", () => "Hello World!");

app.Run();
