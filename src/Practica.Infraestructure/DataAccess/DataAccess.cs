using Practica.Domain.Entities;

namespace Practica.Infraestructure.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private PedidosContext _dbContext;

        public DataAccess(PedidosContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Pedido item)
        {
            await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Pedido>> GetAll()
        {
            return _dbContext.Pedidos;
        }

        public async Task<Pedido> GetById(Guid id) => _dbContext.Pedidos.FirstOrDefault(x => x.Id == id);

        public async Task Update(Pedido item)
        {
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
