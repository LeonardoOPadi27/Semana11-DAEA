using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers;

public class EventosController : Controller
{
    private readonly EventoRepositorio _repositorio;
    public EventosController(EventoRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Index(int? categoriaId)
    {
        await CargarCategoriasAsync(categoriaId);
        return View(await _repositorio.ObtenerActivosAsync(categoriaId));
    }

    public async Task<IActionResult> Details(int id)
    {
        var evento = await _repositorio.ObtenerPorIdAsync(id);
        return evento is null ? NotFound() : View(evento);
    }

    public async Task<IActionResult> Create() { await CargarCategoriasAsync(); return View(new Evento { Fecha = DateTime.Today.AddDays(1).AddHours(18), Modalidad = "Presencial" }); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Evento evento)
    {
        ModelState.Remove(nameof(Evento.Categoria)); ModelState.Remove(nameof(Evento.Inscripciones));
        if (!ModelState.IsValid) { await CargarCategoriasAsync(evento.CategoriaId); return View(evento); }
        await _repositorio.CrearAsync(evento); TempData["Mensaje"] = "Evento creado correctamente."; return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id) { var evento = await _repositorio.ObtenerPorIdAsync(id); if (evento is null) return NotFound(); await CargarCategoriasAsync(evento.CategoriaId); return View(evento); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Evento evento)
    {
        if (id != evento.Id) return NotFound();
        ModelState.Remove(nameof(Evento.Categoria)); ModelState.Remove(nameof(Evento.Inscripciones));
        if (!ModelState.IsValid) { await CargarCategoriasAsync(evento.CategoriaId); return View(evento); }
        await _repositorio.ActualizarAsync(evento); TempData["Mensaje"] = "Evento actualizado correctamente."; return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id) { var evento = await _repositorio.ObtenerPorIdAsync(id); return evento is null ? NotFound() : View(evento); }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        TempData[await _repositorio.BajaLogicaAsync(id) ? "Mensaje" : "Error"] = "Evento dado de baja correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCategoriasAsync(int? seleccionada = null) => ViewBag.Categorias = new SelectList(await _repositorio.ObtenerCategoriasActivasAsync(), "Id", "Nombre", seleccionada);
}
