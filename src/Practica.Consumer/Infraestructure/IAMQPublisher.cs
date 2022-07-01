using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Practica.Consumer.Infraestructure
{
    public interface IAMQPublisher
    {
        Task SendMessage(string topic, object data);
    }
}