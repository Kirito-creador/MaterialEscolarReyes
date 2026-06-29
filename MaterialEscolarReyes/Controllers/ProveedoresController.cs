using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================
        // LISTAR
        //=========================
        public async Task<IActionResult> Index(string buscar)
        {
            var proveedores = _context.Proveedores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                proveedores = proveedores.Where(p =>
                    p.Nombre.Contains(buscar) ||
                    (p.Correo != null && p.Correo.Contains(buscar)));
            }

            return View(await proveedores
                .OrderBy(p => p.Nombre)
                .ToListAsync());
        }

        //=========================
        // DETALLE
        //=========================
        public async Task<IActionResult> Details(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        //=========================
        // CREAR
        //=========================
        public IActionResult Create()
        {
            return View(new Proveedor());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            _context.Proveedores.Add(proveedor);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Proveedor registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        //=========================
        // EDITAR
        //=========================
        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            var proveedorDB = await _context.Proveedores.FindAsync(proveedor.Id);

            if (proveedorDB == null)
                return NotFound();

            proveedorDB.Nombre = proveedor.Nombre;
            proveedorDB.Telefono = proveedor.Telefono;
            proveedorDB.Correo = proveedor.Correo;
            proveedorDB.Direccion = proveedor.Direccion;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Proveedor actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        //=========================
        // ELIMINAR
        //=========================
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return RedirectToAction(nameof(Index));

            _context.Proveedores.Remove(proveedor);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "Proveedor eliminado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}