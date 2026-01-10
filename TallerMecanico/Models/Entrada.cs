using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerMecanico.Models;

public class Entrada
{
    [Key]
    public int EntradaId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now; 

    [Required(ErrorMessage = "El concepto es obligatorio")]
    public string Concepto { get; set; } = string.Empty; 

    public double Total { get; set; } 

    [ForeignKey("EntradaId")]
    public virtual ICollection<EntradaDetalle> Detalles { get; set; } = new List<EntradaDetalle>();
}
