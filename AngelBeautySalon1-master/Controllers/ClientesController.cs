using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngelBeautySalon1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AngelBeautySalon.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var clientes = from c in _context.Clientes
                          select c;

            // Búsqueda interactiva
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(c => c.Nombre.Contains(searchString)
                                            || c.Apellido.Contains(searchString)
                                            || c.Telefono.Contains(searchString)
                                            || c.Email.Contains(searchString));
            }

            return View(await clientes.ToListAsync());
        }

        // GET: Clientes/BuscarAjax
        [HttpGet]
        public async Task<IActionResult> BuscarAjax(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new { success = false, message = "Término de búsqueda vacío" });
            }

            var clientes = await _context.Clientes
                .Where(c => c.Nombre.Contains(term) 
                         || c.Apellido.Contains(term)
                         || c.Telefono.Contains(term)
                         || c.Email.Contains(term))
                .Select(c => new
                {
                    id = c.ClienteId,
                    nombre = c.Nombre + " " + c.Apellido,
                    telefono = c.Telefono,
                    email = c.Email
                })
                .Take(10)
                .ToListAsync();

            return Json(new { success = true, data = clientes });
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Nombre,Apellido,Telefono,Email,Direccion,FechaNacimiento")] Cliente cliente)
        {
            // Validaciones personalizadas del lado del servidor
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                ModelState.AddModelError("Apellido", "El apellido es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                ModelState.AddModelError("Telefono", "El teléfono es obligatorio");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(cliente.Telefono, @"^\d{8,10}$"))
            {
                ModelState.AddModelError("Telefono", "El teléfono debe tener entre 8 y 10 dígitos");
            }

            if (string.IsNullOrWhiteSpace(cliente.Email))
            {
                ModelState.AddModelError("Email", "El email es obligatorio");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(cliente.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError("Email", "El formato del email no es válido");
            }

            // Verificar si el email ya existe
            var emailExiste = await _context.Clientes.AnyAsync(c => c.Email == cliente.Email);
            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Este email ya está registrado");
            }

            if (cliente.FechaNacimiento > DateTime.Now.AddYears(-18))
            {
                ModelState.AddModelError("FechaNacimiento", "El cliente debe ser mayor de 18 años");
            }

            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nombre,Apellido,Telefono,Email,Direccion,FechaNacimiento")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            // Validaciones (igual que en Create)
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                ModelState.AddModelError("Apellido", "El apellido es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                ModelState.AddModelError("Telefono", "El teléfono es obligatorio");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(cliente.Telefono, @"^\d{8,10}$"))
            {
                ModelState.AddModelError("Telefono", "El teléfono debe tener entre 8 y 10 dígitos");
            }

            if (string.IsNullOrWhiteSpace(cliente.Email))
            {
                ModelState.AddModelError("Email", "El email es obligatorio");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(cliente.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError("Email", "El formato del email no es válido");
            }

            // Verificar si el email ya existe (excepto el actual)
            var emailExiste = await _context.Clientes.AnyAsync(c => c.Email == cliente.Email && c.ClienteId != id);
            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Este email ya está registrado");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cliente actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
