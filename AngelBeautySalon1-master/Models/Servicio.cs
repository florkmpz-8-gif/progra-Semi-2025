using AngelBeautySalon1.Models;
using System.ComponentModel.DataAnnotations;

namespace AngelBeautySalon1.Models
{
    public class Servicio
    {
        [Key]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe estar entre $0.01 y $10,000")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(5, 480, ErrorMessage = "La duración debe estar entre 5 y 480 minutos")]
        public int DuracionMinutos { get; set; }

        public bool Activo { get; set; } = true;

        [StringLength(50)]
        public string? Categoria { get; set; }

        // Relación con Citas
        public virtual ICollection<Cita>? Citas { get; set; }

        // Propiedad calculada para mostrar duración
        public string DuracionTexto => $"{DuracionMinutos} minutos";
    }
}