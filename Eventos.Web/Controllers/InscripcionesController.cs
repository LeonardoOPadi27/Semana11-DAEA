using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers;

public class InscripcionesController : Controller
{
    private readonly InscripcionRepositorio _repositorio;
    public InscripcionesController(InscripcionRepositorio repositorio) => _repositorio = repositorio;
    public async Task<IActionResult> Index() => View(await _repositorio.ObtenerTodasAsync());
    public async Task<IActionResult> Create() { await CargarListasAsync(); return View(new Inscripcion()); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Inscripcion inscripcion)
    {
        ModelState.Remove(nameof(Inscripcion.Evento)); ModelState.Remove(nameof(Inscripcion.Asistente));
        var evento = await _repositorio.ObtenerEventoAsync(inscripcion.EventoId);
        if (evento is null || evento.Fecha <= DateTime.Now) ModelState.AddModelError(nameof(Inscripcion.EventoId), "El evento debe estar activo y no haber pasado.");
        else
        {
            if (await _repositorio.TieneInscripcionConfirmadaAsync(inscripcion.EventoId, inscripcion.AsistenteId)) ModelState.AddModelError(nameof(Inscripcion.AsistenteId), "El asistente ya tiene una inscripción confirmada en este evento.");
            if (await _repositorio.ContarConfirmadasAsync(inscripcion.EventoId) >= evento.Aforo) ModelState.AddModelError(nameof(Inscripcion.EventoId), "El evento ya alcanzó su aforo máximo.");
        }
        if (!ModelState.IsValid) { await CargarListasAsync(inscripcion.EventoId, inscripcion.AsistenteId); return View(inscripcion); }
        await _repositorio.CrearAsync(inscripcion); TempData["Mensaje"] = "Inscripción confirmada correctamente."; return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var cancelada = await _repositorio.CancelarAsync(id);
        TempData[cancelada ? "Mensaje" : "Error"] = cancelada ? "Inscripción cancelada correctamente." : "La inscripción no puede cancelarse.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(int? eventoSeleccionado = null, int? asistenteSeleccionado = null)
    {
        var eventos = await _repositorio.ObtenerEventosActivosAsync(); var asistentes = await _repositorio.ObtenerAsistentesActivosAsync();
        ViewBag.Eventos = new SelectList(eventos.Select(e => new { e.Id, Nombre = $"{e.Titulo} · {e.Fecha:dd/MM/yyyy}" }), "Id", "Nombre", eventoSeleccionado);
        ViewBag.Asistentes = new SelectList(asistentes, "Id", "Nombres", asistenteSeleccionado);
    }
}
