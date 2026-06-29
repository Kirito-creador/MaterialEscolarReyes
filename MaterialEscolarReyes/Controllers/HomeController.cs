using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
namespace MaterialEscolarReyes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tarjetas del Dashboard
            ViewBag.Productos = _context.Productos.Count();
            ViewBag.Clientes = _context.Clientes.Count();
            ViewBag.Proveedores = _context.Proveedores.Count();
            ViewBag.Ventas = _context.Ventas.Count();

            // Últimos clientes
            ViewBag.UltimosClientes = _context.Clientes
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToList();

            // Últimos productos
            ViewBag.UltimosProductos = _context.Productos
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToList();

            // Productos con stock bajo
            var stockBajos = _context.Productos
                .Where(p => p.Stock <= p.StockMinimo)
                .OrderBy(p => p.Stock)
                .ToList();

            ViewBag.StockBajos = stockBajos;
            ViewBag.StockBajo = stockBajos.Count;
            // =========================
            // TOP 5 PRODUCTOS MÁS VENDIDOS
            // =========================
            ViewBag.ProductosMasVendidos = _context.DetalleVentas
                .GroupBy(d => d.Producto!.Nombre)
                .Select(g => new
                {
                    Producto = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToList();
            // =========================
            // VENTAS POR MES
            // =========================

            var ventasMes = _context.Ventas
                .GroupBy(v => v.Fecha.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.Mes)
                .ToList();

            string[] nombresMeses =
            {
    "",
    "Ene",
    "Feb",
    "Mar",
    "Abr",
    "May",
    "Jun",
    "Jul",
    "Ago",
    "Sep",
    "Oct",
    "Nov",
    "Dic"
};

            ViewBag.Meses = System.Text.Json.JsonSerializer.Serialize(
                ventasMes.Select(x => nombresMeses[x.Mes]).ToList());

            ViewBag.Cantidades = System.Text.Json.JsonSerializer.Serialize(
                ventasMes.Select(x => x.Cantidad).ToList());

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
