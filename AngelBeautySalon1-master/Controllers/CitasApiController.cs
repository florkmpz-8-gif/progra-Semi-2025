using Microsoft.AspNetCore.Mvc;
using AngelBeautySalon1.Models;
using System.Collections.Generic;
using System.Linq;

namespace AngelBeautySalon1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CitasApi
        [HttpGet]
        public ActionResult<IEnumerable<Cita>> GetCitas()
        {
            var citas = _context.Citas.ToList();
            return Ok(citas);
        }

        // GET: api/CitasApi/5
        [HttpGet("{id}")]
        public ActionResult<Cita> GetCita(int id)
        {
            var cita = _context.Citas.Find(id);
            if (cita == null)
            {
                return NotFound(new { mensaje = "Cita no encontrada" });
            }
            return Ok(cita);
        }

        // GET: api/CitasApi/hoy
        [HttpGet("hoy")]
        public ActionResult<IEnumerable<Cita>> GetCitasHoy()
        {
            var hoy = DateTime.Now.Date;
            var citas = _context.Citas
                .Where(c => c.Fecha.Date == hoy)
                .ToList();
            return Ok(citas);
        }

        // POST: api/CitasApi
        [HttpPost]
        public ActionResult<Cita> PostCita(Cita cita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Citas.Add(cita);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCita), new { id = cita.IdCita }, cita);
        }

        // PUT: api/CitasApi/5
        [HttpPut("{id}")]
        public IActionResult PutCita(int id, Cita cita)
        {
            if (id != cita.IdCita)
            {
                return BadRequest(new { mensaje = "ID no coincide" });
            }

            var citaExistente = _context.Citas.Find(id);
            if (citaExistente == null)
            {
                return NotFound(new { mensaje = "Cita no encontrada" });
            }

            citaExistente.NombreCliente = cita.NombreCliente;
            citaExistente.Servicio = cita.Servicio;
            citaExistente.Fecha = cita.Fecha;
            citaExistente.Hora = cita.Hora;
            citaExistente.Estado = cita.Estado;

            _context.SaveChanges();
            return Ok(new { mensaje = "Cita actualizada", cita = citaExistente });
        }

        // PUT: api/CitasApi/5/completar
        [HttpPut("{id}/completar")]
        public IActionResult CompletarCita(int id)
        {
            var cita = _context.Citas.Find(id);
            if (cita == null)
            {
                return NotFound(new { mensaje = "Cita no encontrada" });
            }

            cita.Estado = "Completada";
            _context.SaveChanges();

            return Ok(new { mensaje = "Cita marcada como completada", cita });
        }

        // DELETE: api/CitasApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCita(int id)
        {
            var cita = _context.Citas.Find(id);
            if (cita == null)
            {
                return NotFound(new { mensaje = "Cita no encontrada" });
            }

            _context.Citas.Remove(cita);
            _context.SaveChanges();

            return Ok(new { mensaje = "Cita eliminada exitosamente" });
        }
    }
}
