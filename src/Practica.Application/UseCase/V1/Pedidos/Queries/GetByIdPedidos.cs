using MediatR;
using Practica.Domain.Entities;
using Practica.Infraestructure.DataAccess;

namespace Practica.Application.UseCase.V1.Pedidos.Queries
{
    public class QueryGetByIdPedido : IRequest<Pedido>
    {
        public Guid Id { get; set; }
    }

    public class GetByIdPedidoHandler : IRequestHandler<QueryGetByIdPedido, Pedido>
    {
        private IDataAccess _dataAcess;

        public GetByIdPedidoHandler(IDataAccess dataAccess)
        {
            _dataAcess = dataAccess;
        }

        public async Task<Pedido> Handle(QueryGetByIdPedido request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataAcess.GetById(request.Id);

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}