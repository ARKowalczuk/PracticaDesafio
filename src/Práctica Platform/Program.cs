using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
using Practica.API.Controllers.V1;
using Practica.API.Workers.V1;
using Practica.Application.Boopstrap;
using Practica.Application.UseCase.V1.Pedidos.Command;
using Practica.Application.UseCase.V1.Pedidos.Queries;
using Practica.Domain.Entities;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfraestructueServices(options => options.ConnectionString = builder.Configuration.GetConnectionString("PedidosDatabase"));
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(CreatePedidoCommand).Assembly);

builder.Services.AddHostedService<AsignarPedidoWorker>();


Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));
Log.Information("Starting Microservice... ");
Log.Information($"Name [{Assembly.GetEntryAssembly().GetName().Name}] Version [{Assembly.GetEntryAssembly().GetName().Version}]");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.PracticaControllerV1();

app.Run();



