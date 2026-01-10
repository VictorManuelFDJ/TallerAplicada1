using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Data;
using TallerMecanico.Models;

namespace TallerMecanico.Services;

public class ProductosService(IDbContextFactory<ApplicationDbContext> factory)
{
    public async Task<Productos?> Buscar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductoId == id);
    }

    public async Task<List<Productos>> Listar(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Productos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Guardar(Productos producto)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        if (producto.ProductoId == 0)
            contexto.Productos.Add(producto);
        else
            contexto.Update(producto);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Productos
            .Where(p => p.ProductoId == id)
            .ExecuteDeleteAsync() > 0;
    }
}