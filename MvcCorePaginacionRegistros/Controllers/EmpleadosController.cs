using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class EmpleadosController : Controller
    {
        private HospitalRepository repo;

        public EmpleadosController(HospitalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult>
            EmpleadosDepartamento(int iddepartamento)
        {
            List<Empleado> empleados =
                await this.repo.GetEmpleadosDepartamentoAsync(iddepartamento);
            return View(empleados);
        }

    }
}
