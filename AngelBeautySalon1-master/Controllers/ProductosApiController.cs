using Microsoft.AspNetCore.Mvc;
using AngelBeautySalon1.Models;
using System.Collections.Generic;
using System.Linq;

namespace AngelBeautySalon1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductosApi
        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetProductos()
        {
            var productos = _context.Productos.ToList();
            return Ok(productos);
        }

        // GET: api/ProductosApi/5
        [HttpGet("{id}")]
        public ActionResult<Producto> GetProducto(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }
            return Ok(producto);
        }

        // GET: api/ProductosApi/stock-bajo
        [HttpGet("stock-bajo")]
        public ActionResult<IEnumerable<Producto>> GetProductosStockBajo()
        {
            var productos = _context.Productos
                .Where(p => p.Stock < 10)
                .ToList();
            return Ok(productos);
        }

        // POST: api/ProductosApi
        [HttpPost]
        public ActionResult<Producto> PostProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Productos.Add(producto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.IdProductos }, producto);
        }

        // PUT: api/ProductosApi/5
        [HttpPut("{id}")]
        public IActionResult PutProducto(int id, Producto producto)
        {
            if (id != producto.IdProducto) 
            {
                return BadRequest(new { mensaje = "ID no coincide" });
            }

            var productoExistente = _context.Productos.Find(id);
            if (productoExistente == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            productoExistente.Categoria = producto.Categoria;

            _context.SaveChanges();
            return Ok(new { mensaje = "Producto actualizado", producto = productoExistente });
        }

        // PUT: api/ProductosApi/5/actualizar-stock
        [HttpPut("{id}/actualizar-stock")]
        public IActionResult ActualizarStock(int id, [FromBody] int cantidad)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            producto.Stock += cantidad;
            _context.SaveChanges();

            return Ok(new { mensaje = "Stock actualizado", producto });
        }

        // DELETE: api/ProductosApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProducto(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return Ok(new { mensaje = "Producto eliminado exitosamente" });
        }
    }
}
