using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;

public class EventoRepositorio
{
    private readonly EventosContext _context;
    public EventoRepositorio(EventosContext context) => _context = context;

    public async Task<List<Evento>> ObtenerActivosAsync(int? categoriaId)
    {
        var query = _context.Eventos.AsNoTracking().Include(e => e.Categoria).Include(e => e.Inscripciones.Where(i => i.Estado == "Confirmada")).Where(e => e.Activo);
        if (categoriaId.HasValue) query = query.Where(e => e.CategoriaId == categoriaId.Value);
        return await query.OrderBy(e => e.Fecha).ToListAsync();
    }

    public Task<Evento?> ObtenerPorIdAsync(int id) => _context.Eventos.AsNoTracking().Include(e => e.Categoria).Include(e => e.Inscripciones).FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    public Task<List<Categoria>> ObtenerCategoriasActivasAsync() => _context.Categorias.AsNoTracking().Where(c => c.Activo).OrderBy(c => c.Nombre).ToListAsync();
    public async Task CrearAsync(Evento evento) { evento.Activo = true; _context.Eventos.Add(evento); await _context.SaveChangesAsync(); }
    public async Task ActualizarAsync(Evento evento)
    {
        var actual = await _context.Eventos.FindAsync(evento.Id);
        if (actual is null) return;
        actual.Titulo = evento.Titulo; actual.Fecha = evento.Fecha; actual.Lugar = evento.Lugar; actual.Modalidad = evento.Modalidad; actual.Aforo = evento.Aforo; actual.Precio = evento.Precio; actual.CategoriaId = evento.CategoriaId;
        await _context.SaveChangesAsync();
    }
    public async Task<bool> BajaLogicaAsync(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento is null || !evento.Activo) return false;
        evento.Activo = false; await _context.SaveChangesAsync(); return true;
    }
}
