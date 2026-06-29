using MaterialEscolarReyes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class TiendaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiendaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar)
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.Stock > 0)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.ToLower();

                productos = productos.Where(p =>
                    p.Nombre.ToLower().Contains(buscar) ||
                    p.Codigo.ToLower().Contains(buscar) ||
                    p.Categoria!.Nombre.ToLower().Contains(buscar));
            }

            ViewBag.Categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View("Index", await productos.ToListAsync());
        }
        public async Task<IActionResult> Producto(int id)
        {
            var producto = await _context.Productos

                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Proveedor)

                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return NotFound();

            ViewBag.Relacionados = await _context.Productos

                .Where(p => p.CategoriaId == producto.CategoriaId &&
                            p.Id != producto.Id)

                .Take(4)

                .ToListAsync();

            return View(producto);
        }
        //=========================
        // TODOS LOS PRODUCTOS
        //=========================
        public async Task<IActionResult> Productos(string buscar)
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.Stock > 0)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.ToLower();

                productos = productos.Where(p =>
                    p.Nombre.ToLower().Contains(buscar) ||
                    p.Codigo.ToLower().Contains(buscar) ||
                    p.Categoria!.Nombre.ToLower().Contains(buscar));
            }

            ViewBag.Buscar = buscar;

            ViewBag.Categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View("Productos", await productos.ToListAsync());
        }
        //=========================
        // LISTA DE CATEGORÍAS
        //=========================
        public async Task<IActionResult> Categorias()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Estado)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View(categorias);
        }
        //=========================
        // PRODUCTOS POR CATEGORÍA
        //=========================
        public async Task<IActionResult> Categoria(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.CategoriaId == id && p.Stock > 0)
                .ToListAsync();

            ViewBag.Categoria = categoria;
            return View("Categoria", productos);
        }
        public async Task<IActionResult> Ofertas()
        {
            var productos = await _context.Productos
                .Where(p => p.Stock > 0)
                .OrderByDescending(p => p.FechaRegistro)
                .Take(12)
                .ToListAsync();

            ViewBag.Categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View("Index", productos);
        }
        public IActionResult Contacto()
        {
            return View();
        }
    }
}