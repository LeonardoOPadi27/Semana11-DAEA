using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Eventos.Web.Controllers;

public class AsistentesController : Controller
{
    private readonly AsistenteRepositorio _repositorio;
    public AsistentesController(AsistenteRepositorio repositorio) => _repositorio = repositorio;
    public async Task<IActionResult> Index() => View(await _repositorio.ObtenerActivosAsync());
    public IActionResult Create() => View(new Asistente());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Asistente asistente)
    {
        ModelState.Remove(nameof(Asistente.Inscripciones));
        if (await _repositorio.DniExisteAsync(asistente.Dni)) ModelState.AddModelError(nameof(Asistente.Dni), "El DNI ya está registrado.");
        if (await _repositorio.CorreoExisteAsync(asistente.Correo)) ModelState.AddModelError(nameof(Asistente.Correo), "El correo ya está registrado.");
        if (!ModelState.IsValid) return View(asistente);
        await _repositorio.CrearAsync(asistente); TempData["Mensaje"] = "Asistente registrado correctamente."; return RedirectToAction(nameof(Index));
    }
}
