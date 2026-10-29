using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;

public class InscripcionRepositorio
{
    private readonly EventosContext _context;
    public InscripcionRepositorio(EventosContext context) => _context = context;
    public Task<List<Evento>> ObtenerEventosActivosAsync() => _context.Eventos.AsNoTracking().Where(e => e.Activo).OrderBy(e => e.Fecha).ToListAsync();
    public Task<List<Asistente>> ObtenerAsistentesActivosAsync() => _context.Asistentes.AsNoTracking().Where(a => a.Activo).OrderBy(a => a.Nombres).ToListAsync();
    public Task<Evento?> ObtenerEventoAsync(int id) => _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    public Task<bool> TieneInscripcionConfirmadaAsync(int eventoId, int asistenteId) => _context.Inscripciones.AsNoTracking().AnyAsync(i => i.EventoId == eventoId && i.AsistenteId == asistenteId && i.Estado == "Confirmada");
    public Task<int> ContarConfirmadasAsync(int eventoId) => _context.Inscripciones.AsNoTracking().CountAsync(i => i.EventoId == eventoId && i.Estado == "Confirmada");
    public async Task CrearAsync(Inscripcion inscripcion) { inscripcion.Estado = "Confirmada"; inscripcion.FechaInscripcion = DateTime.Now; _context.Inscripciones.Add(inscripcion); await _context.SaveChangesAsync(); }
    public Task<List<Inscripcion>> ObtenerTodasAsync() => _context.Inscripciones.AsNoTracking().Include(i => i.Evento).Include(i => i.Asistente).OrderByDescending(i => i.FechaInscripcion).ToListAsync();
    public async Task<bool> CancelarAsync(int id) { var inscripcion = await _context.Inscripciones.FindAsync(id); if (inscripcion is null || inscripcion.Estado != "Confirmada") return false; inscripcion.Estado = "Cancelada"; await _context.SaveChangesAsync(); return true; }
}
