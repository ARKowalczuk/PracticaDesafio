using Confluent.Kafka;
using MediatR;
using Practica.Application.UseCase.V1.Pedidos.Command;
using Practica.Domain.Entities;
using Practica.Infraestructure.DataAccess;
using System.Text.Json;

namespace Practica.API.Workers.V1
{
    public class AsignarPedidoWorker : BackgroundService
    {
        public record AMQConsumerConfig(string BootstrapServers, string GroupId, int AutoOffsetReset);

        private readonly ILogger<AsignarPedidoWorker> _logger;
        private ConsumerConfig _config;
        private IMediator _mediator;

        public AsignarPedidoWorker(ILogger<AsignarPedidoWorker> logger, IConfiguration configuration, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            AMQConsumerConfig rawconfig = new(null, null, 0);
            configuration.GetSection("AMQConsumerConfig").Bind(rawconfig);

            _config = new ConsumerConfig
            {
                BootstrapServers = rawconfig.BootstrapServers,
                GroupId = rawconfig.GroupId,
                AutoOffsetReset = (AutoOffsetReset)rawconfig.AutoOffsetReset
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => Subscribe(stoppingToken));
        }

        private async Task Subscribe(CancellationToken stoppingToken)
        {
            var topics = new List<string>() { "pedido-asignado" };
            var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();

            // nos subscribimos a los topicos
            consumer.Subscribe(topics);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // escuchamos si hay mensjaes
                    var consumeResult = consumer.Consume(stoppingToken);

                    // manejamos los mensajes.
                    _logger.LogInformation(
                        consumeResult.Message.Timestamp.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + $": [{consumeResult.Message.Value}]");

                    var pedido = JsonSerializer.Deserialize<Pedido>(consumeResult.Message.Value);
                    await _mediator.Send(new UpdatePedidoCommand() { Id = pedido.Id, NumeroDePedido = pedido.NumeroDePedido.Value, Estado = "ASIGNADO" });
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"an error occured: {ex.Error.Reason}", ex);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error asignando pedido", e);
                }
            }

            consumer.Close();
        }
    }
}