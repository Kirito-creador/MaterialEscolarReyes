using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using MaterialEscolarReyes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class CuentaClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Cliente> _hasher;

        public CuentaClienteController(ApplicationDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<Cliente>();
        }

        //=========================
        // LOGIN
        //=========================
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Correo == model.Correo);

            if (cliente == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            var resultado = _hasher.VerifyHashedPassword(
                cliente,
                cliente.Password,
                model.Password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            HttpContext.Session.SetInt32("ClienteId", cliente.Id);
            HttpContext.Session.SetString("ClienteNombre", cliente.Nombre);

            return RedirectToAction("Index", "Tienda");
        }

        //=========================
        // REGISTRO
        //=========================
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool existe = await _context.Clientes
                .AnyAsync(c => c.Correo == model.Correo);

            if (existe)
            {
                ModelState.AddModelError("", "El correo ya está registrado.");
                return View(model);
            }

            Cliente cliente = new Cliente
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Correo = model.Correo,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                Estado = true,
                FechaRegistro = DateTime.UtcNow
            };

            cliente.Password = _hasher.HashPassword(cliente, model.Password);

            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Cuenta creada correctamente.";

            return RedirectToAction(nameof(Login));
        }

        //=========================
        // PERFIL
        //=========================
        public async Task<IActionResult> Perfil()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
                return RedirectToAction("Login");

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
                return RedirectToAction("Login");

            return View(cliente);
        }

        //=========================
        // LOGOUT
        //=========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Tienda");
        }
        public async Task<IActionResult> MisPedidos()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
                return RedirectToAction("Login");

            var pedidos = await _context.Pedidos
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return View(pedidos);
        }
        public async Task<IActionResult> DetallePedido(int id)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
                return RedirectToAction("Login");

            bool pedidoExiste = await _context.Pedidos
                .AnyAsync(p => p.Id == id && p.ClienteId == clienteId);

            if (!pedidoExiste)
                return NotFound();

            var detalles = await _context.DetallePedidos
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            return View(detalles);
        }
    }
}