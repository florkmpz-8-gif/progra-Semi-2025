using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngelBeautySalon1.Models
{
    public class Cita
    {
        [Key]
        public int CitaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ServicioId { get; set; }

        public string Notas { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Confirmada, Completada, Cancelada

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones de navegación
        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        [ForeignKey("ServicioId")]
        public virtual Servicio? Servicio { get; set; }

        // Propiedad calculada
        public DateTime FechaHoraCompleta => Fecha.Date + Hora;
    }
}