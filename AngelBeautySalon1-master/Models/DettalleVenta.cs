using System.ComponentModel.DataAnnotations;

namespace AngelBeautySalon1.Models
{
    public class DetalleVenta
    {
        [Key]
        public int DetalleVentaId { get; set; }

        [Required]
        public int VentaId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }

        // Propiedades de navegación
        public Venta? Venta { get; set; }
        public Producto? Producto { get; set; }
    }
}