using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class DepartamentosController : Controller
    {
        private HospitalRepository repo;
        public DepartamentosController(HospitalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Details(int id, int? posicion)
        {
            if (posicion == null) posicion = 1;
            ModelPaginacionDepartamentoEmpleados model =
                await this.repo.GetEmpleadosByDeptAsync(id, posicion.Value);
            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["POSICION"] = posicion;
            return View(model);
        }
    }
}
