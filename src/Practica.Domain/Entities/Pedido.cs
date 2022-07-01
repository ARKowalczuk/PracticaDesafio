using System.ComponentModel.DataAnnotations;

namespace Practica.Domain.Entities
{
    public class Pedido
    {
        [Key]
        public Guid Id { get; set; }
        public long? NumeroDePedido { get; set; }
        public Guid CicloDelPedido { get; set; }
        public long CodigoDeContratoInterno { get; set; }
        public string EstadoDelPedido { get; set; }
        public long CuentaCorriente { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Cuando { get; set; }
    }

}