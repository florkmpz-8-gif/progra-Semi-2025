using AngelBeautySalon1.Models;
using System.ComponentModel.DataAnnotations;

namespace AngelBeautySalon1.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe estar entre $0.01 y $10,000")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, 10000, ErrorMessage = "El stock debe estar entre 0 y 10,000")]
        public int Stock { get; set; }

        [Range(0, 10000)]
        public int StockMinimo { get; set; } = 5;

        [StringLength(50)]
        public string? Marca { get; set; }

        [StringLength(50)]
        public string? Categoria { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Propiedad calculada para alerta de stock
        public bool StockBajo => Stock <= StockMinimo;

        // Relación con Ventas
        public virtual ICollection<Venta>? Ventas { get; set; }
    }
}