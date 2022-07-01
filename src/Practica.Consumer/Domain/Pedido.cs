namespace Practica.Consumer.Domain
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public long? NumeroDePedido { get; set; }
        public Guid CicloDelPedido { get; set; }
        public int CodigoDeContratoInterno { get; set; }
        public string EstadoDelPedido { get; set; }
        public long CuentaCorriente { get; set; }
        public DateTime Cuando { get; set; }
    }
}