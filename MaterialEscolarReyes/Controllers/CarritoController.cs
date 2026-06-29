using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using MaterialEscolarReyes.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
            {
                return RedirectToAction("Login", "CuentaCliente");
            }

            var carrito = await _context.Carritos
                .Include(c => c.Producto)
                .ThenInclude(p => p.Categoria)
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();

            return View(carrito);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(int id, int cantidad)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
            {
                return Json(new
                {
                    ok = false,
                    login = true
                });
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return Json(new
                {
                    ok = false
                });
            }

            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(c =>
                    c.ClienteId == clienteId &&
                    c.ProductoId == id);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    ClienteId = clienteId.Value,
                    ProductoId = id,
                    Cantidad = cantidad,
                    Fecha = DateTime.UtcNow
                };

                _context.Carritos.Add(carrito);
            }
            else
            {
                carrito.Cantidad += cantidad;

                _context.Carritos.Update(carrito);
            }

            await _context.SaveChangesAsync();

            int total = await _context.Carritos
                .Where(c => c.ClienteId == clienteId)
                .SumAsync(c => c.Cantidad);

            return Json(new
            {
                ok = true,
                total
            });
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
            {
                return Json(new { ok = false });
            }

            var item = await _context.Carritos
                .FirstOrDefaultAsync(c => c.Id == id && c.ClienteId == clienteId);

            if (item == null)
            {
                return Json(new { ok = false });
            }

            _context.Carritos.Remove(item);

            await _context.SaveChangesAsync();

            return Json(new { ok = true });
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int id, int cantidad)
        {
            try
            {
                var item = await _context.Carritos
                    .Include(c => c.Producto)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (item == null)
                    return Json(new { ok = false });

                if (cantidad <= 0)
                {
                    _context.Carritos.Remove(item);
                }
                else
                {
                    item.Cantidad = cantidad;

                    // Actualizar la fecha en formato UTC
                    item.Fecha = DateTime.UtcNow;

                    _context.Carritos.Update(item);
                }

                await _context.SaveChangesAsync();

                decimal subtotal = item.Producto != null
                    ? item.Producto.PrecioVenta * item.Cantidad
                    : 0;

                int? clienteId = HttpContext.Session.GetInt32("ClienteId");

                decimal total = await _context.Carritos
                    .Include(c => c.Producto)
                    .Where(c => c.ClienteId == clienteId)
                    .SumAsync(c => c.Producto!.PrecioVenta * c.Cantidad);

                return Json(new
                {
                    ok = true,
                    subtotal,
                    total
                });
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        //=========================
        // CHECKOUT
        //=========================
        public async Task<IActionResult> Checkout()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
                return RedirectToAction("Login", "CuentaCliente");

            var cliente = await _context.Clientes.FindAsync(clienteId);

            var carrito = await _context.Carritos
                .Include(c => c.Producto)
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();

            if (!carrito.Any())
                return RedirectToAction("Index");

            CheckoutViewModel model = new CheckoutViewModel
            {
                Direccion = cliente?.Direccion ?? "",
                Telefono = cliente?.Telefono ?? "",
                Total = carrito.Sum(c => c.Producto!.PrecioVenta * c.Cantidad)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
                return RedirectToAction("Login", "CuentaCliente");

            var carrito = await _context.Carritos
                .Include(c => c.Producto)
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();
            foreach (var item in carrito)
            {
                if (item.Producto == null)
                {
                    ModelState.AddModelError("", "Existe un producto inválido en el carrito.");
                    return View(model);
                }

                if (item.Producto.Stock < item.Cantidad)
                {
                    ModelState.AddModelError("", $"No hay suficiente stock de {item.Producto.Nombre}.");
                    return View(model);
                }
            }

            if (!carrito.Any())
                return RedirectToAction("Index");

            decimal total = carrito.Sum(c => c.Producto!.PrecioVenta * c.Cantidad);

            Pedido pedido = new Pedido
            {
                ClienteId = clienteId.Value,
                Fecha = DateTime.UtcNow,
                Total = total,
                Estado = "Pendiente",
                Direccion = model.Direccion,
                Telefono = model.Telefono,
                MetodoPago = model.MetodoPago
            };

            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();
            // Crear la venta
            Venta venta = new Venta
            {
                NumeroVenta = "V-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                Fecha = DateTime.UtcNow,
                ClienteId = clienteId.Value,
                UsuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 1,
                Subtotal = total,
                Descuento = 0,
                Total = total
            };

            _context.Ventas.Add(venta);

            await _context.SaveChangesAsync();

            foreach (var item in carrito)
            {
                if (item.Producto!.Stock < item.Cantidad)
                {
                    ModelState.AddModelError("", $"No hay suficiente stock de {item.Producto.Nombre}");
                    return View(model);
                }

                // Detalle del pedido
                DetallePedido detallePedido = new DetallePedido
                {
                    PedidoId = pedido.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    Precio = item.Producto.PrecioVenta,
                    Subtotal = item.Producto.PrecioVenta * item.Cantidad
                };

                _context.DetallePedidos.Add(detallePedido);

                // Detalle de la venta
                DetalleVenta detalleVenta = new DetalleVenta
                {
                    VentaId = venta.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Producto.PrecioVenta,
                    Subtotal = item.Producto.PrecioVenta * item.Cantidad
                };

                _context.DetalleVentas.Add(detalleVenta);

                // Descontar stock
                item.Producto.Stock -= item.Cantidad;
            }

            _context.Carritos.RemoveRange(carrito);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CompraExitosa));
        }
        public IActionResult CompraExitosa()
        {
            return View();
        }

    }
}