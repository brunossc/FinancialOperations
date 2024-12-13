using FinancialOperations.API;
using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Services;
using FinancialOperations.API.Infrastructure.Repositories;
using FinancialOperations.API.Middleware;
using FinancialOperations.SideCar;
using MongoDB.Driver;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddSerilogWithElastic("FinancialOperations.API", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
builder.Host.UseSerilog();

builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
    new MongoClient(builder.Configuration.GetConnectionString("NoSQLConnection")));

builder.Services.AddMassTransitExtensions(builder.Configuration);

builder.Services.AddScoped<IOperationRepository, OperationRepository>();
builder.Services.AddScoped<IOperationsService, OperationsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseMiddleware<ErrorHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
