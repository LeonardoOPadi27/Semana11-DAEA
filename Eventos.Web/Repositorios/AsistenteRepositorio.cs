using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;

public class AsistenteRepositorio
{
    private readonly EventosContext _context;
    public AsistenteRepositorio(EventosContext context) => _context = context;
    public Task<List<Asistente>> ObtenerActivosAsync() => _context.Asistentes.AsNoTracking().Where(a => a.Activo).OrderBy(a => a.Nombres).ToListAsync();
    public Task<bool> DniExisteAsync(string dni) => _context.Asistentes.AnyAsync(a => a.Dni == dni);
    public Task<bool> CorreoExisteAsync(string correo) => _context.Asistentes.AnyAsync(a => a.Correo == correo);
    public async Task CrearAsync(Asistente asistente) { asistente.Activo = true; _context.Asistentes.Add(asistente); await _context.SaveChangesAsync(); }
}
