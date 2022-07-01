using System.ComponentModel.DataAnnotations;

public class PedidoCreateRequest
{
    [Required, RegularExpression("([0-9]*)")]
    public string CuentaCorriente { get; set; }


    [Required, RegularExpression("([0-9]*)")]
    public string CodigoDeContratoInterno { get; set; }

}
