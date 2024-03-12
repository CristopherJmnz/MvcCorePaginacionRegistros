using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;
using System.Diagnostics.Metrics;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        private HospitalRepository repo;

        public PaginacionController(HospitalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult>
            PaginarRegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                //PONEMOS LA POSICION EN EL PRIMER REGISTRO
                posicion = 1;
            }
            //PRIMERO = 1
            //SIGUIENTE = 5
            //ANTERIOR = 4
            //ULTIMO = 5
            int numeroRegistros =
                await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            int siguiente = posicion.Value + 1;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > numeroRegistros)
            {
                //EFECTO OPTICO
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            VistaDepartamento vista = await
                this.repo.GetVistaDepartamentoAsync(posicion.Value);
            ViewData["ULTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(vista);
        }
        public async Task<IActionResult>
            PaginarGrupoVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            //<a href='paginargrupo?posicion=1'>Pagina 1</a>
            //<a href='paginargrupo?posicion=3'>Pagina 2</a>
            //<a href='paginargrupo?posicion=5'>Pagina 3</a>
            //<a href='paginargrupo?posicion=7'>Pagina 4</a>
            int numeroRegistros = await
                this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            ViewData["REGISTROS"] = numeroRegistros;
            List<VistaDepartamento> departamentos =
                await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }

        public async Task<IActionResult>
            PaginarGrupoDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            //<a href='paginargrupo?posicion=1'>Pagina 1</a>
            //<a href='paginargrupo?posicion=3'>Pagina 2</a>
            //<a href='paginargrupo?posicion=5'>Pagina 3</a>
            //<a href='paginargrupo?posicion=7'>Pagina 4</a>
            int numeroRegistros = await
                this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            ViewData["REGISTROS"] = numeroRegistros;
            List<Departamento> departamentos =
                await this.repo.GetGrupoDepartamentosAsync(posicion.Value);
            return View(departamentos);
        }


        public async Task<IActionResult> PaginarGrupoEmpleados(int? posicion)
        {
            int numeroRegistros = await
                this.repo.GetNumeroRegistrosEmpleadosAsync();
            if (posicion == null) posicion = 1;
            if (posicion != null && posicion > numeroRegistros)
            {
                posicion = numeroRegistros - 2;
            }
            ViewData["REGISTROS"] = numeroRegistros;
            List<Empleado> empleados = await this.repo.GetGrupoEmpleadosAsync(posicion.Value);
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosOficio(int? posicion, string? oficio)
        {
            if (posicion == null) posicion = 1;
            if (oficio != null && posicion != null)
            {
                List<Empleado> empleados = await
                    this.repo.GetGrupoEmpleadosByOficioAsync(posicion.Value, oficio);
                ViewData["REGISTROS"] = await this.repo.GetNumeroEmpleadosByOficio(oficio);
                ViewData["OFICIO"] = oficio;
                return View(empleados);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmpleadosOficio(string oficio)
        {
            List<Empleado> empleados = await
                this.repo.GetGrupoEmpleadosByOficioAsync(1, oficio);
            int registros = await this.repo.GetNumeroEmpleadosByOficio(oficio);
            ViewData["REGISTROS"] = registros;
            ViewData["OFICIO"] = oficio;
            return View(empleados);
        }




        public async Task<IActionResult> EmpleadosOficioOut(int? posicion, string oficio)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                ModelPaginacionEmpleados model = await this.repo.
                    GetGrupoEmpleadosByOficioOutAsync(posicion.Value, oficio);
                ViewData["REGISTROS"] = model.NumeroRegistros;
                ViewData["OFICIO"] = oficio;
                return View(model.Empleados);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EmpleadosOficioOut(string oficio)
        {
            ModelPaginacionEmpleados model = await this.repo.
                    GetGrupoEmpleadosByOficioOutAsync(1, oficio);
            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["OFICIO"] = oficio;
            return View(model.Empleados);
        }
    }
}
