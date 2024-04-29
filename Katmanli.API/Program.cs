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



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(Katmanli.Core.SharedLibrary.Logging.ConfigureLogging);



// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(MapProfile));

//Servis Kayýtlarý
builder.Services.AddScoped<ITokenCreator, TokenCreator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<DatabaseExecutions, DatabaseExecutions>();
builder.Services.AddScoped<ParameterList>();
builder.Services.AddScoped<MainHub,MainHub>();

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

builder.Services.AddSignalR();


var app = builder.Build();


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

app.Run();

