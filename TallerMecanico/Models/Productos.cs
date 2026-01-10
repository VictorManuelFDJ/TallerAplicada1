using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; } 

    [Required(ErrorMessage = "La descripción es requerida")]
    public string Descripcion { get; set; } = string.Empty; 

    [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
    public double Costo { get; set; } 

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public double Precio { get; set; } 

    public int Existencia { get; set; } 
}