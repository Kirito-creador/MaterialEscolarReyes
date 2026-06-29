using MaterialEscolarReyes.Data;
using MaterialEscolarReyes.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == model.Correo);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario no encontrado";
                return View(model);
            }

            bool ok = usuario.Password == model.Password;

            if (!ok)
            {
                ViewBag.Error = "Contraseña incorrecta";
                return View(model);
            }
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            HttpContext.Session.SetString("Usuario", usuario.Nombre);

            HttpContext.Session.SetString("Rol", usuario.Rol);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}