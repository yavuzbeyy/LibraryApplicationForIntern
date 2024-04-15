using Katmanli.Core.Interfaces.DataAccessInterfaces;
using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.Entities;
using Katmanli.DataAccess.Repository;
using Katmanli.Service.Mapping;
using Katmanli.Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(MapProfile));

//Servis Kayýtlarý
//builder.Services.AddScoped<IGenericRepository<User>,GenericRepository<User>>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITokenCreator, TokenCreator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<DatabaseExecutions, DatabaseExecutions>();

//DbContext Ekleme
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

