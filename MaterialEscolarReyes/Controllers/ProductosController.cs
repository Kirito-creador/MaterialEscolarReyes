using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaterialEscolarReyes.ViewModels;
using System.IO;

namespace MaterialEscolarReyes.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductosController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //=========================
        // LISTAR
        //=========================
        public async Task<IActionResult> Index(string buscar)
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Proveedor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                productos = productos.Where(p =>
                    p.Nombre.Contains(buscar) ||
                    p.Codigo.Contains(buscar));
            }

            return View(await productos
                .OrderByDescending(p => p.Id)
                .ToListAsync());
        }

        //=========================
        // CREAR
        //=========================
        public IActionResult Create()
        {
            int siguiente = 1;

            if (_context.Productos.Any())
            {
                siguiente = _context.Productos.Max(x => x.Id) + 1;
            }

            ProductoViewModel model = new ProductoViewModel();

            model.Codigo = $"PRO{siguiente:000000}";

            ViewBag.Categorias =
                new SelectList(_context.Categorias, "Id", "Nombre");

            ViewBag.Marcas =
                new SelectList(_context.Marcas, "Id", "Nombre");

            ViewBag.Proveedores =
                new SelectList(_context.Proveedores, "Id", "Nombre");

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
                ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre");
                ViewBag.Proveedores = new SelectList(_context.Proveedores, "Id", "Nombre");

                return View(model);
            }

            string? imagenPrincipal = null;
            string? imagenSecundaria = null;

            string carpeta = Path.Combine(_environment.WebRootPath, "uploads", "productos");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            // Imagen principal
            if (model.ImagenPrincipal != null)
            {
                imagenPrincipal = Guid.NewGuid() +
                                  Path.GetExtension(model.ImagenPrincipal.FileName);

                string ruta = Path.Combine(carpeta, imagenPrincipal);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await model.ImagenPrincipal.CopyToAsync(stream);
                }
            }

            // Imagen secundaria
            if (model.ImagenSecundaria != null)
            {
                imagenSecundaria = Guid.NewGuid() +
                                   Path.GetExtension(model.ImagenSecundaria.FileName);

                string ruta = Path.Combine(carpeta, imagenSecundaria);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await model.ImagenSecundaria.CopyToAsync(stream);
                }
            }

            Producto producto = new Producto
            {
                Codigo = model.Codigo,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,

                PrecioCompra = model.PrecioCompra,
                PrecioVenta = model.PrecioVenta,

                Stock = model.Stock,
                StockMinimo = model.StockMinimo,

                CategoriaId = model.CategoriaId,
                MarcaId = model.MarcaId,
                ProveedorId = model.ProveedorId,

                ImagenPrincipal = imagenPrincipal,
                ImagenSecundaria = imagenSecundaria,

                FechaRegistro = DateTime.UtcNow
            };

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Producto registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        //=========================
        // EDITAR
        //=========================
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            ProductoViewModel model = new ProductoViewModel
            {
                Id = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,

                PrecioCompra = producto.PrecioCompra,
                PrecioVenta = producto.PrecioVenta,

                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,

                CategoriaId = producto.CategoriaId,
                MarcaId = producto.MarcaId,
                ProveedorId = producto.ProveedorId,

                ImagenPrincipalActual = producto.ImagenPrincipal,
                ImagenSecundariaActual = producto.ImagenSecundaria
            };

            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", model.CategoriaId);
            ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre", model.MarcaId);
            ViewBag.Proveedores = new SelectList(_context.Proveedores, "Id", "Nombre", model.ProveedorId);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", model.CategoriaId);
                ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre", model.MarcaId);
                ViewBag.Proveedores = new SelectList(_context.Proveedores, "Id", "Nombre", model.ProveedorId);

                return View(model);
            }

            var producto = await _context.Productos.FindAsync(model.Id);

            if (producto == null)
                return NotFound();

            string carpeta = Path.Combine(_environment.WebRootPath, "uploads", "productos");

            // Imagen principal
            if (model.ImagenPrincipal != null)
            {
                if (!string.IsNullOrEmpty(producto.ImagenPrincipal))
                {
                    string vieja = Path.Combine(carpeta, producto.ImagenPrincipal);

                    if (System.IO.File.Exists(vieja))
                        System.IO.File.Delete(vieja);
                }

                string nombre = Guid.NewGuid() + Path.GetExtension(model.ImagenPrincipal.FileName);

                using (var stream = new FileStream(Path.Combine(carpeta, nombre), FileMode.Create))
                {
                    await model.ImagenPrincipal.CopyToAsync(stream);
                }

                producto.ImagenPrincipal = nombre;
            }

            // Imagen secundaria
            if (model.ImagenSecundaria != null)
            {
                if (!string.IsNullOrEmpty(producto.ImagenSecundaria))
                {
                    string vieja = Path.Combine(carpeta, producto.ImagenSecundaria);

                    if (System.IO.File.Exists(vieja))
                        System.IO.File.Delete(vieja);
                }

                string nombre = Guid.NewGuid() + Path.GetExtension(model.ImagenSecundaria.FileName);

                using (var stream = new FileStream(Path.Combine(carpeta, nombre), FileMode.Create))
                {
                    await model.ImagenSecundaria.CopyToAsync(stream);
                }

                producto.ImagenSecundaria = nombre;
            }

            producto.Codigo = model.Codigo;
            producto.Nombre = model.Nombre;
            producto.Descripcion = model.Descripcion;

            producto.PrecioCompra = model.PrecioCompra;
            producto.PrecioVenta = model.PrecioVenta;

            producto.Stock = model.Stock;
            producto.StockMinimo = model.StockMinimo;

            producto.CategoriaId = model.CategoriaId;
            producto.MarcaId = model.MarcaId;
            producto.ProveedorId = model.ProveedorId;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Producto actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        //=========================
        // ELIMINAR
        //=========================
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return RedirectToAction(nameof(Index));

            string carpeta = Path.Combine(_environment.WebRootPath, "uploads", "productos");

            if (!string.IsNullOrEmpty(producto.ImagenPrincipal))
            {
                string ruta = Path.Combine(carpeta, producto.ImagenPrincipal);

                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
            }

            if (!string.IsNullOrEmpty(producto.ImagenSecundaria))
            {
                string ruta = Path.Combine(carpeta, producto.ImagenSecundaria);

                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
            }

            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Producto eliminado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}