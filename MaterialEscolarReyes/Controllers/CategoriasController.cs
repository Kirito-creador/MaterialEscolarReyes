using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using MaterialEscolarReyes.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MaterialEscolarReyes.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoriasController(
      ApplicationDbContext context,
      IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        // LISTAR Y BUSCAR
        public async Task<IActionResult> Index(string buscar)
        {
            var categorias = _context.Categorias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                categorias = categorias.Where(c => c.Nombre.Contains(buscar));
            }

            return View(await categorias.OrderBy(c => c.Nombre).ToListAsync());
        }

        // CREAR
        public IActionResult Create()
        {
            return View(new CategoriaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? nombreImagen = null;

            if (model.Imagen != null)
            {
                string carpeta = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "categorias");

                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                nombreImagen = Guid.NewGuid().ToString() +
                               Path.GetExtension(model.Imagen.FileName);

                string ruta = Path.Combine(carpeta, nombreImagen);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await model.Imagen.CopyToAsync(stream);
                }
            }

            Categoria categoria = new Categoria
            {
                Nombre = model.Nombre,
                Estado = model.Estado,
                Imagen = nombreImagen,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Categorias.Add(categoria);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Categoría registrada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // EDITAR
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            CategoriaViewModel model = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Estado = categoria.Estado,
                ImagenActual = categoria.Imagen
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoriaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var categoria = await _context.Categorias.FindAsync(model.Id);

            if (categoria == null)
                return NotFound();

            categoria.Nombre = model.Nombre;
            categoria.Estado = model.Estado;

            if (model.Imagen != null)
            {
                string carpeta = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "categorias");

                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                // Eliminar imagen anterior
                if (!string.IsNullOrEmpty(categoria.Imagen))
                {
                    string rutaAnterior = Path.Combine(carpeta, categoria.Imagen);

                    if (System.IO.File.Exists(rutaAnterior))
                    {
                        System.IO.File.Delete(rutaAnterior);
                    }
                }

                string nombreImagen = Guid.NewGuid().ToString() +
                                      Path.GetExtension(model.Imagen.FileName);

                string rutaNueva = Path.Combine(carpeta, nombreImagen);

                using (var stream = new FileStream(rutaNueva, FileMode.Create))
                {
                    await model.Imagen.CopyToAsync(stream);
                }

                categoria.Imagen = nombreImagen;
            }

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Categoría actualizada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return RedirectToAction(nameof(Index));

            if (!string.IsNullOrEmpty(categoria.Imagen))
            {
                string ruta = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "categorias",
                    categoria.Imagen);

                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }
            }

            _context.Categorias.Remove(categoria);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Categoría eliminada correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}