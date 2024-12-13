using FinancialOperations.Consolidator.API;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using FinancialOperations.Consolidator.API.Domain.Services;
using FinancialOperations.Consolidator.API.Domain.Services.Interfaces;
using FinancialOperations.Consolidator.API.Infrastructure.Repositories;
using FinancialOperations.Consolidator.API.Services;
using FinancialOperations.SideCar;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSerilogWithElastic("FinancialOperations.Consolidator.API", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
builder.Host.UseSerilog();

builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
    new MongoClient(builder.Configuration.GetConnectionString("NoSQLConnection")));

builder.Services.AddMassTransitExtensions(builder.Configuration);

builder.Services.AddScoped<IOperationRepository, OperationRepository>();
builder.Services.AddScoped<IProcessedOperationRepository, ProcessedOperationRepository>();
builder.Services.AddScoped<IProcessOperation, ProcessOperation>();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<OperationService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
