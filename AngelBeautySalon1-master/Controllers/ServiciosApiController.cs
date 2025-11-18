using Microsoft.AspNetCore.Mvc;
using AngelBeautySalon1.Models;
using System.Collections.Generic;
using System.Linq;

namespace AngelBeautySalon1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiciosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiciosApi
        [HttpGet]
        public ActionResult<IEnumerable<Servicio>> GetServicios()
        {
            var servicios = _context.Servicios.ToList();
            return Ok(servicios);
        }

        // GET: api/ServiciosApi/5
        [HttpGet("{id}")]
        public ActionResult<Servicio> GetServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
            {
                return NotFound(new { mensaje = "Servicio no encontrado" });
            }
            return Ok(servicio);
        }

        // GET: api/ServiciosApi/categoria/Cabello
        [HttpGet("categoria/{categoria}")]
        public ActionResult<IEnumerable<Servicio>> GetServiciosPorCategoria(string categoria)
        {
            var servicios = _context.Servicios
                .Where(s => s.Categoria == categoria)
                .ToList();
            return Ok(servicios);
        }

        // POST: api/ServiciosApi
        [HttpPost]
        public ActionResult<Servicio> PostServicio(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Servicios.Add(servicio);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetServicio), new { id = servicio.ServicioId }, servicio);
        }

        // PUT: api/ServiciosApi/5
        [HttpPut("{id}")]
        public IActionResult PutServicio(int id, Servicio servicio)
        {
            if (id != servicio.ServicioId)
            {
                return BadRequest(new { mensaje = "ID no coincide" });
            }

            var servicioExistente = _context.Servicios.Find(id);
            if (servicioExistente == null)
            {
                return NotFound(new { mensaje = "Servicio no encontrado" });
            }

            servicioExistente.Nombre = servicio.Nombre;
            servicioExistente.Descripcion = servicio.Descripcion;
            servicioExistente.Precio = servicio.Precio;
            servicioExistente.DuracionMinutos = servicio.DuracionMinutos;
            servicioExistente.Categoria = servicio.Categoria;

            _context.SaveChanges();
            return Ok(new { mensaje = "Servicio actualizado", servicio = servicioExistente });
        }

        // DELETE: api/ServiciosApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
            {
                return NotFound(new { mensaje = "Servicio no encontrado" });
            }

            _context.Servicios.Remove(servicio);
            _context.SaveChanges();

            return Ok(new { mensaje = "Servicio eliminado exitosamente" });
        }
    }
}
