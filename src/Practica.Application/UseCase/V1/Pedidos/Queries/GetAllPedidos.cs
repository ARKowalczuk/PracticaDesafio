using MediatR;
using Practica.Domain.Entities;
using Practica.Infraestructure.DataAccess;

namespace Practica.Application.UseCase.V1.Pedidos.Queries
{
    public class QueryGetAllPedidos : IRequest<IEnumerable<Pedido>>
    {

    }

    public class GetAllPedidosHandler : IRequestHandler<QueryGetAllPedidos, IEnumerable<Pedido>>
    {
        private IDataAccess _dataAcess;

        public GetAllPedidosHandler(IDataAccess dataAccess)
        {
            _dataAcess = dataAccess;
        }

        public async Task<IEnumerable<Pedido>> Handle(QueryGetAllPedidos request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataAcess.GetAll();

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}