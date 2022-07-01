using MediatR;
using Practica.Consumer;
using Practica.Consumer.Application.UseCase.V1;
using Practica.Consumer.Infraestructure;
using Practica.Consumer.Workers;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;


Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumirPedidoCreadoWorker>();
        services.AddSingleton<IAMQPublisher, AMQPublisher>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
    })

    .UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration))
    .Build();


Log.Information("Starting Microservice... ");
Log.Information($"Name [{Assembly.GetEntryAssembly().GetName().Name}] Version [{Assembly.GetEntryAssembly().GetName().Version}]");


await host.RunAsync();
