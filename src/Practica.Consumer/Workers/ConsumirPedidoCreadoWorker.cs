using Confluent.Kafka;
using MediatR;
using Practica.Consumer.Application.UseCase.V1;
using Practica.Consumer.Domain;
using Practica.Consumer.Infraestructure;
using System.Text.Json;

namespace Practica.Consumer.Workers
{
    public class ConsumirPedidoCreadoWorker : BackgroundService
    {
        public record AMQConsumerConfig(string BootstrapServers, string GroupId, int AutoOffsetReset);

        private readonly ILogger<ConsumirPedidoCreadoWorker> _logger;

        private IMediator _mediator;
        private ConsumerConfig _config;

        public ConsumirPedidoCreadoWorker(ILogger<ConsumirPedidoCreadoWorker> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;

            AMQConsumerConfig rawconfig = new(null, null, 0);
            configuration.GetSection(nameof(AMQConsumerConfig)).Bind(rawconfig);

            _config = new ConsumerConfig
            {
                BootstrapServers = rawconfig.BootstrapServers,
                GroupId = rawconfig.GroupId,
                AutoOffsetReset = (AutoOffsetReset)rawconfig.AutoOffsetReset
            };

            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => Subscribe(stoppingToken));
        }

        private async Task Subscribe(CancellationToken stoppingToken)
        {
            var topics = new List<string>() { "test-documentation" };
            // Consumer builder<Tkey, TValue> el value es el evento que estamos esperando.
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
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
                            $"MR -> {consumeResult.Message.Timestamp.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss")} : [{consumeResult.Message.Value}]");
                        var pedido = JsonSerializer.Deserialize<Pedido>(consumeResult.Message.Value);
                        await _mediator.Send(new AsignarPedidoCommand() { Pedido = pedido }, stoppingToken);

                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"an error occured: {ex.Error.Reason}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error asignando el pedido", e);
                    }
                }

                consumer.Close();
            };
        }
    }
}