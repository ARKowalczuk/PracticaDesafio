using MediatR;
using Microsoft.Extensions.Logging;
using Practica.Domain.Entities;
using Practica.Infraestructure.BrokerService;
using Practica.Infraestructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Application.UseCase.V1.Pedidos.Command
{
    public class CreatePedidoCommand : IRequest<Pedido>
    {
        public PedidoCreateRequest Pedido { get; set; }
    }

    public class CreatePedidoHandler : IRequestHandler<CreatePedidoCommand, Pedido>
    {
        private IDataAccess _dataAcess;
        private IAMQPublisher _publisher;
        private ILogger<CreatePedidoHandler> _logger;

        public CreatePedidoHandler(ILogger<CreatePedidoHandler> logger, IDataAccess dataAccess, IAMQPublisher publisher)
        {
            _dataAcess = dataAccess;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<Pedido> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var identifier = Guid.NewGuid();
                var item = new Pedido()
                {
                    Id = identifier,
                    NumeroDePedido = null,
                    CicloDelPedido = identifier,
                    EstadoDelPedido = null,
                    CuentaCorriente = Convert.ToInt32(request.Pedido.CuentaCorriente),
                    CodigoDeContratoInterno = Convert.ToInt32(request.Pedido.CodigoDeContratoInterno),
                    Cuando = DateTime.Now
                };
                
                await _dataAcess.Add(item);

                _logger.LogInformation($"Pedido creado ID: [{item.Id}] Cuenta corriente [{item.CuentaCorriente}] Codigo contrato interno [{item.CodigoDeContratoInterno}]");
                await _publisher.SendMessage(item);
                return item;
            }
            catch (Exception e)
            {
                _logger.LogError("Error creating pedido", e);
                throw;
            }
        }
    }
}
