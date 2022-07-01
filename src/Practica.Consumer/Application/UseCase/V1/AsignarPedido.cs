using MediatR;
using Practica.Consumer.Domain;
using Practica.Consumer.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Consumer.Application.UseCase.V1
{

    public class AsignarPedidoCommand : IRequest
    {
        public Pedido Pedido { get; set; }
    }

    public class AsignarPedidoHandler : IRequestHandler<AsignarPedidoCommand, Unit>
    {
        private readonly IAMQPublisher _publisher;
        private ILogger<AsignarPedidoHandler> _logger;

        public AsignarPedidoHandler(ILogger<AsignarPedidoHandler> logger, IAMQPublisher publisher)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(AsignarPedidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.Pedido.NumeroDePedido = (new Random()).Next();
                _logger.LogInformation($"Pedido asignado ID: [{request.Pedido.Id}] Numero pedido [{request.Pedido.NumeroDePedido}]");
                await _publisher.SendMessage("pedido-asignado", request.Pedido);
                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError("Error al asignar el numero de pedido", e);
                throw;
            }
        }
    }
}
