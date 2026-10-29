using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Models;

[Index(nameof(Dni), IsUnique = true)]
[Index(nameof(Correo), IsUnique = true)]
public class Asistente
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required, StringLength(8), RegularExpression(@"^\d{8}$")]
    public string Dni { get; set; } = string.Empty;

    [Required, StringLength(100), EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [StringLength(9)]
    public string? Telefono { get; set; }

    public bool Activo { get; set; } = true;
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
