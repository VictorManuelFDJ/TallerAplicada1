using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TallerMecanico.Data;
using TallerMecanico.Models;

namespace TallerMecanico.Services;

public class EntradasService(IDbContextFactory<ApplicationDbContext> factory)
{
    public async Task<bool> Guardar(Entrada entrada)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        
        await using var transaccion = await contexto.Database.BeginTransactionAsync();

        try
        {
            var guardado = entrada.EntradaId == 0
                ? await Insertar(entrada, contexto)
                : await Modificar(entrada, contexto);

            if (guardado)
            {
                await contexto.SaveChangesAsync();
                await transaccion.CommitAsync();
            }
            return guardado;
        }
        catch (Exception)
        {
            await transaccion.RollbackAsync();
            return false;
        }
    }

    private async Task<bool> Insertar(Entrada entrada, ApplicationDbContext contexto)
    {
        foreach (var detalle in entrada.Detalles)
        {
            var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
            if (producto != null)
                producto.Existencia += detalle.Cantidad;
        }
        contexto.Entradas.Add(entrada);
        return true;
    }

    private async Task<bool> Modificar(Entrada entrada, ApplicationDbContext contexto)
    {
        var entradaAnterior = await contexto.Entradas
            .Include(e => e.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);

        if (entradaAnterior == null) return false;

        foreach (var detalle in entradaAnterior.Detalles)
        {
            var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
            if (producto != null)
                producto.Existencia -= detalle.Cantidad;
        }

        contexto.EntradaDetalles.RemoveRange(contexto.EntradaDetalles.Where(d => d.EntradaId == entrada.EntradaId));

        foreach (var detalle in entrada.Detalles)
        {
            var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
            if (producto != null)
                producto.Existencia += detalle.Cantidad;
        }

        contexto.Update(entrada);
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        var entrada = await contexto.Entradas
            .Include(e => e.Detalles)
            .FirstOrDefaultAsync(e => e.EntradaId == id);

        if (entrada == null) return false;

        foreach (var detalle in entrada.Detalles)
        {
            var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
            if (producto != null)
                producto.Existencia -= detalle.Cantidad; 
        }

        contexto.Entradas.Remove(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Entrada?> Buscar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == id); 
    }

    public async Task<List<Entrada>> Listar(Expression<Func<Entrada, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.Detalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync(); 
    }
}