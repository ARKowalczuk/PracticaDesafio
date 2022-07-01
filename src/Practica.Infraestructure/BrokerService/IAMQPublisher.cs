using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Practica.Infraestructure.BrokerService
{
    public interface IAMQPublisher
    {
        Task SendMessage(object data);
    }
}