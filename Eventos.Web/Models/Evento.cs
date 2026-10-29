using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventos.Web.Models;

public class Evento
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }

    [Required, StringLength(100)]
    public string Lugar { get; set; } = string.Empty;

    [Required, StringLength(20), RegularExpression("^(Presencial|Virtual)$")]
    public string Modalidad { get; set; } = "Presencial";

    [Range(1, 5000)]
    public int Aforo { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public bool Activo { get; set; } = true;
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
