using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================
        // LISTAR CLIENTES
        //=========================
        public async Task<IActionResult> Index(string buscar)
        {
            var clientes = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                clientes = clientes.Where(c =>
                    c.Nombre.Contains(buscar) ||
                    c.Correo.Contains(buscar));
            }

            return View(await clientes
                .OrderBy(c => c.Nombre)
                .ToListAsync());
        }
        //=========================
        // DETALLE
        //=========================
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }
        //=========================
        // EDITAR
        //=========================
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            var clienteDB = await _context.Clientes.FindAsync(cliente.Id);

            if (clienteDB == null)
                return NotFound();

            clienteDB.Nombre = cliente.Nombre;
            clienteDB.Apellido = cliente.Apellido;
            clienteDB.Correo = cliente.Correo;
            clienteDB.Telefono = cliente.Telefono;
            clienteDB.Direccion = cliente.Direccion;
            clienteDB.Estado = cliente.Estado;

            // Mantener la contraseña y la fecha de registro originales
            // No las modificamos aquí.

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Cliente actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        //=========================
        // ELIMINAR
        //=========================
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return RedirectToAction(nameof(Index));

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Cliente eliminado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}