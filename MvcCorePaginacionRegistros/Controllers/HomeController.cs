using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using System.Diagnostics;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario)
        {
            HttpContext.Session.SetString("USUARIO", usuario);
            ViewData["MENSAJE"] = "Usuario validado";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USUARIO");
            return RedirectToAction("Index");
        }

    }
}
