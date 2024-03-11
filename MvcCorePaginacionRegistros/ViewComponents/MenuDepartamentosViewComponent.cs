using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.ViewComponents
{
    public class MenuDepartamentosViewComponent : ViewComponent
    {
        private HospitalRepository repo;

        public MenuDepartamentosViewComponent(HospitalRepository repo)
        {
            this.repo = repo;
        }   

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Departamento> departamentos =
                await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
    }

}
