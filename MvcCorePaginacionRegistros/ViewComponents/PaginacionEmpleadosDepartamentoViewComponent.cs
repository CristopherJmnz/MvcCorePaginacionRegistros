using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.ViewComponents
{
    public class PaginacionEmpleadosDepartamentoViewComponent :ViewComponent
    {
        private HospitalRepository repo;
        public PaginacionEmpleadosDepartamentoViewComponent(HospitalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, int posicion)
        {
            ModelPaginacionDepartamentoEmpleados model = await
                this.repo.GetEmpleadosByDeptAsync(id, posicion);
            return View(model);
        }
    }
}
