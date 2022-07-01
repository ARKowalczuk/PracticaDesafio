using System.ComponentModel.DataAnnotations;

public class PedidoUpdateRequest
{
    [Required, Range(1, int.MaxValue)]
    public int NumeroDePedido { get; set; }
}
