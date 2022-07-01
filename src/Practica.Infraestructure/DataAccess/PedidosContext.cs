using Microsoft.EntityFrameworkCore;
using Practica.Domain.Entities;

namespace Practica.Infraestructure.DataAccess
{
    public class PedidosContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }

        public PedidosContext(DbContextOptions options) : base(options)
        {

        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>();
        }
        #endregion
    }
}
