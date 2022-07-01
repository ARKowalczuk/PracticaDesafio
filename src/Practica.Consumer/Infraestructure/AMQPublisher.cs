using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Practica.Consumer.Infraestructure
{
    public record AMQPublisherConfig(string BootstrapServers, int SecurityProtocol);

    public class AMQPublisher : IAMQPublisher
    {
        ProducerConfig _config;
        ILogger<AMQPublisher> _logger;

        public AMQPublisher(ILogger<AMQPublisher> logger, IConfiguration configuration)
        {
            AMQPublisherConfig rawconfig = new(null, 0);
            configuration.GetSection(nameof(AMQPublisherConfig)).Bind(rawconfig);

            _config = new ProducerConfig
            {
                BootstrapServers = rawconfig.BootstrapServers,
                SecurityProtocol = (SecurityProtocol)rawconfig.SecurityProtocol,
            };

            _logger = logger;
        }

        public async Task SendMessage(string topic, object data)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                // creamos el mensaje <Key, Value>, el Value será nuestro evento.
                var message = new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(data)
                };
                try
                {
                    // publicamos el evento.
                    var dr = await producer.ProduceAsync(topic, message);
                    _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error sending message to Kafka", ex);
                }
            }
        }
    }
}
