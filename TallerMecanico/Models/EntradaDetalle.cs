using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.Models;

public class EntradaDetalle
{
    [Key]
    public int Id { get; set; } 

    public int EntradaId { get; set; } 

    [Required(ErrorMessage = "Debe seleccionar un producto")]
    public int ProductoId { get; set; } 

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; } 

    public double Costo { get; set; }
}
