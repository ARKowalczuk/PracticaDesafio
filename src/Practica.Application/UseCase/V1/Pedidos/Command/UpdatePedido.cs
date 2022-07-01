using MediatR;
using Microsoft.Extensions.Logging;
using Practica.Domain.Entities;
using Practica.Infraestructure.DataAccess;

namespace Practica.Application.UseCase.V1.Pedidos.Command
{
    public class UpdatePedidoCommand : IRequest<Pedido>
    {
        public Guid Id { get; set; }
        public long NumeroDePedido { get; set; }
        public string? Estado { get; set; }
    }


    public class UpdatePedidoHandler : IRequestHandler<UpdatePedidoCommand, Pedido>
    {
        private IDataAccess _dataAcess;
        private ILogger<UpdatePedidoHandler> _logger;

        public UpdatePedidoHandler(ILogger<UpdatePedidoHandler> logger, IDataAccess dataAccess)
        {
            _dataAcess = dataAccess;
            _logger = logger;
        }

        public async Task<Pedido> Handle(UpdatePedidoCommand request, CancellationToken cancellationToken)
        {
            var item = await _dataAcess.GetById(request.Id);

            ArgumentNullException.ThrowIfNull(item);

            item.NumeroDePedido = request.NumeroDePedido;

            if (request.Estado != null) item.EstadoDelPedido = request.Estado;

            item.Cuando = DateTime.Now;
            await _dataAcess.Update(item);

            _logger.LogInformation($"Pedido actualizado Id: [{item.Id}] Numero de pedido [{item.NumeroDePedido}] Estado[{item.EstadoDelPedido}]");
            return item;
        }
    }
}
