using MaterialEscolarReyes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================
        // LISTAR VENTAS
        //=========================
        public async Task<IActionResult> Index(string buscar)
        {
            var ventas = _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                ventas = ventas.Where(v =>
                    v.NumeroVenta.Contains(buscar) ||
                    v.Cliente!.Nombre.Contains(buscar));
            }

            return View(await ventas
                .OrderByDescending(v => v.Fecha)
                .ToListAsync());
        }

        //=========================
        // DETALLE
        //=========================
        public async Task<IActionResult> Details(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles!)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound();

            return View(venta);
        }
    }
}