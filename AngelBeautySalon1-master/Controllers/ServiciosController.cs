using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngelBeautySalon.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AngelBeautySalon.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Servicios
        public async Task<IActionResult> Index(string searchString, decimal? precioMin, decimal? precioMax, string categoria)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["PrecioMin"] = precioMin;
            ViewData["PrecioMax"] = precioMax;
            ViewData["Categoria"] = categoria;

            var servicios = from s in _context.Servicios
                           select s;

            // Búsqueda interactiva
            if (!String.IsNullOrEmpty(searchString))
            {
                servicios = servicios.Where(s => s.Nombre.Contains(searchString)
                                              || s.Descripcion.Contains(searchString)
                                              || s.Categoria.Contains(searchString));
            }

            if (precioMin.HasValue)
            {
                servicios = servicios.Where(s => s.Precio >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                servicios = servicios.Where(s => s.Precio <= precioMax.Value);
            }

            if (!String.IsNullOrEmpty(categoria))
            {
                servicios = servicios.Where(s => s.Categoria == categoria);
            }

            return View(await servicios.ToListAsync());
        }

        // GET: Servicios/BuscarAjax
        [HttpGet]
        public async Task<IActionResult> BuscarAjax(string term, string categoria)
        {
            var query = _context.Servicios.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(s => s.Nombre.Contains(term) 
                                      || s.Descripcion.Contains(term)
                                      || s.Categoria.Contains(term));
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(s => s.Categoria == categoria);
            }

            var servicios = await query
                .Select(s => new
                {
                    id = s.ServicioId,
                    nombre = s.Nombre,
                    descripcion = s.Descripcion,
                    precio = s.Precio,
                    duracion = s.DuracionMinutos,
                    categoria = s.Categoria
                })
                .Take(10)
                .ToListAsync();

            return Json(new { success = true, data = servicios });
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServicioId,Nombre,Descripcion,Precio,DuracionMinutos,Categoria")] Servicio servicio)
        {
            // Validaciones personalizadas del lado del servidor
            if (string.IsNullOrWhiteSpace(servicio.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre del servicio es obligatorio");
            }
            else if (servicio.Nombre.Length < 3)
            {
                ModelState.AddModelError("Nombre", "El nombre debe tener al menos 3 caracteres");
            }
            else if (servicio.Nombre.Length > 100)
            {
                ModelState.AddModelError("Nombre", "El nombre no puede exceder 100 caracteres");
            }

            // Verificar si el nombre ya existe
            var nombreExiste = await _context.Servicios.AnyAsync(s => s.Nombre == servicio.Nombre);
            if (nombreExiste)
            {
                ModelState.AddModelError("Nombre", "Ya existe un servicio con este nombre");
            }

            if (string.IsNullOrWhiteSpace(servicio.Descripcion))
            {
                ModelState.AddModelError("Descripcion", "La descripción es obligatoria");
            }
            else if (servicio.Descripcion.Length < 10)
            {
                ModelState.AddModelError("Descripcion", "La descripción debe tener al menos 10 caracteres");
            }

            if (servicio.Precio <= 0)
            {
                ModelState.AddModelError("Precio", "El precio debe ser mayor a cero");
            }
            else if (servicio.Precio > 10000)
            {
                ModelState.AddModelError("Precio", "El precio no puede exceder $10,000");
            }

            if (servicio.DuracionMinutos <= 0)
            {
                ModelState.AddModelError("DuracionMinutos", "La duración debe ser mayor a cero");
            }
            else if (servicio.DuracionMinutos < 15)
            {
                ModelState.AddModelError("DuracionMinutos", "La duración mínima es de 15 minutos");
            }
            else if (servicio.DuracionMinutos > 480)
            {
                ModelState.AddModelError("DuracionMinutos", "La duración máxima es de 8 horas (480 minutos)");
            }

            if (string.IsNullOrWhiteSpace(servicio.Categoria))
            {
                ModelState.AddModelError("Categoria", "La categoría es obligatoria");
            }
            else
            {
                var categoriasValidas = new[] { "Cabello", "Uñas", "Maquillaje", "Facial", "Corporal", "Depilación", "Masajes", "Otro" };
                if (!categoriasValidas.Contains(servicio.Categoria))
                {
                    ModelState.AddModelError("Categoria", "Categoría no válida");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(servicio);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            return View(servicio);
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicioId,Nombre,Descripcion,Precio,DuracionMinutos,Categoria")] Servicio servicio)
        {
            if (id != servicio.ServicioId)
            {
                return NotFound();
            }

            // Validaciones (similares a Create)
            if (string.IsNullOrWhiteSpace(servicio.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre del servicio es obligatorio");
            }
            else if (servicio.Nombre.Length < 3)
            {
                ModelState.AddModelError("Nombre", "El nombre debe tener al menos 3 caracteres");
            }
            else if (servicio.Nombre.Length > 100)
            {
                ModelState.AddModelError("Nombre", "El nombre no puede exceder 100 caracteres");
            }

            // Verificar si el nombre ya existe (excepto el actual)
            var nombreExiste = await _context.Servicios.AnyAsync(s => s.Nombre == servicio.Nombre && s.ServicioId != id);
            if (nombreExiste)
            {
                ModelState.AddModelError("Nombre", "Ya existe un servicio con este nombre");
            }

            if (string.IsNullOrWhiteSpace(servicio.Descripcion))
            {
                ModelState.AddModelError("Descripcion", "La descripción es obligatoria");
            }
            else if (servicio.Descripcion.Length < 10)
            {
                ModelState.AddModelError("Descripcion", "La descripción debe tener al menos 10 caracteres");
            }

            if (servicio.Precio <= 0)
            {
                ModelState.AddModelError("Precio", "El precio debe ser mayor a cero");
            }
            else if (servicio.Precio > 10000)
            {
                ModelState.AddModelError("Precio", "El precio no puede exceder $10,000");
            }

            if (servicio.DuracionMinutos <= 0)
            {
                ModelState.AddModelError("DuracionMinutos", "La duración debe ser mayor a cero");
            }
            else if (servicio.DuracionMinutos < 15)
            {
                ModelState.AddModelError("DuracionMinutos", "La duración mínima es de 15 minutos");
            }
            else if (servicio.DuracionMinutos > 480)
            {
                ModelState.AddModelError("DuracionMinutos", "La duración máxima es de 8 horas");
            }

            if (string.IsNullOrWhiteSpace(servicio.Categoria))
            {
                ModelState.AddModelError("Categoria", "La categoría es obligatoria");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicio);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Servicio actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.ServicioId))
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
            return View(servicio);
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.ServicioId == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            
            // Verificar si el servicio tiene citas asociadas
            var tieneCitas = await _context.Citas.AnyAsync(c => c.ServicioId == id);
            if (tieneCitas)
            {
                TempData["Error"] = "No se puede eliminar el servicio porque tiene citas asociadas";
                return RedirectToAction(nameof(Index));
            }

            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.ServicioId == id);
        }
    }
}
