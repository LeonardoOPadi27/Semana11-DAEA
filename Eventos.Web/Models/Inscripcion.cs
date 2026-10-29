using System.ComponentModel.DataAnnotations;

namespace Eventos.Web.Models;

public class Inscripcion
{
    public int Id { get; set; }
    public int EventoId { get; set; }
    public Evento? Evento { get; set; }
    public int AsistenteId { get; set; }
    public Asistente? Asistente { get; set; }
    public DateTime FechaInscripcion { get; set; }

    [Required, StringLength(12), RegularExpression("^(Confirmada|Cancelada)$")]
    public string Estado { get; set; } = "Confirmada";
}
