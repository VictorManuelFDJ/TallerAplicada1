using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Models; 

namespace TallerMecanico.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    
    public DbSet<Productos> Productos { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<EntradaDetalle> EntradaDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        
        modelBuilder.Entity<Productos>().HasData(
            new Productos { ProductoId = 1, Descripcion = "Aceite de motor", Costo = 1580, Precio = 2000, Existencia = 10 },
            new Productos { ProductoId = 2, Descripcion = "Bujías", Costo = 4200, Precio = 5000, Existencia = 5 },
            new Productos { ProductoId = 3, Descripcion = "Baterías", Costo = 10000, Precio = 12000, Existencia = 4 }
        );
    }
}