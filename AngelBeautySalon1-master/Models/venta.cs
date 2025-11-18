using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngelBeautySalon1.Models
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public int? ProductoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0.01, 100000, ErrorMessage = "El total debe ser mayor a $0")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPago { get; set; } = "Efectivo"; // Efectivo, Tarjeta, Transferencia

        [Range(1, 100)]
        public int Cantidad { get; set; } = 1;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(20)]
        public string Estado { get; set; } = "Completada"; // Completada, Pendiente, Cancelada

        // Relaciones de navegación
        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; }
    }
}