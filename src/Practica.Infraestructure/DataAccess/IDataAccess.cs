using Practica.Domain.Entities;

namespace Practica.Infraestructure.DataAccess
{
    public interface IDataAccess
    {
        Task Add(Pedido item);

        Task<IEnumerable<Pedido>> GetAll();

        Task<Pedido> GetById(Guid id);
        Task Update(Pedido item);
    }
}