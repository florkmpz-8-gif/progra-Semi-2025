using Microsoft.AspNetCore.Mvc;
using AngelBeautySalon1.Models;
using System.Collections.Generic;
using System.Linq;

namespace AngelBeautySalon1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientesApi
        // Obtener todos los clientes
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetClientes()
        {
            var clientes = _context.Clientes.ToList();
            return Ok(clientes);
        }

        // GET: api/ClientesApi/5
        // Obtener un cliente por ID
        [HttpGet("{id}")]
        public ActionResult<Cliente> GetCliente(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            return Ok(cliente);
        }

        // POST: api/ClientesApi
        // Crear un nuevo cliente
        [HttpPost]
        public ActionResult<Cliente> PostCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        // PUT: api/ClientesApi/5
        // Actualizar un cliente existente
        [HttpPut("{id}")]
        public IActionResult PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new { mensaje = "El ID no coincide" });
            }

            var clienteExistente = _context.Clientes.Find(id);
            if (clienteExistente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            // Actualizar propiedades
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Email = cliente.Email;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/ClientesApi/5
        // Eliminar un cliente
        [HttpDelete("{id}")]
        public IActionResult DeleteCliente(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: api/ClientesApi/buscar?termino=maria
        // Buscar clientes
        [HttpGet("buscar")]
        public ActionResult<IEnumerable<Cliente>> BuscarClientes([FromQuery] string termino)
        {
            if (string.IsNullOrEmpty(termino))
            {
                return BadRequest(new { mensaje = "Debe proporcionar un termino de busqueda" });
            }

            var clientes = _context.Clientes
                .Where(c => c.Nombre.Contains(termino) || c.Telefono.Contains(termino))
                .ToList();

            return Ok(clientes);
        }
    }
}
