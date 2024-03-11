using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System.Diagnostics.Metrics;

namespace MvcCorePaginacionRegistros.Repositories
{
    #region VIEWS

    //    create view V_DEPARTAMENTOS_INDIVIDUAL
    //as
    //    select CAST(
    //    ROW_NUMBER() over (ORDER BY DEPT_NO) AS INT) AS POSICION,
    //    ISNULL(DEPT_NO, 0) AS DEPT_NO, DNOMBRE, LOC FROM DEPT
    //go

//    alter VIEW V_GRUPO_EMPLEADOS
//AS

//    select CAST(ROW_NUMBER() over (ORDER BY EMP_NO) AS INT) AS POSICION,
//    ISNULL(EMP_NO, 0) AS EMP_NO, apellido, salario from emp
//GO

    #endregion
    #region PROCEDURE

    //    create procedure SP_GRUPO_DEPARTAMENTOS
    //(@POSICION INT)
    //AS

    //    select* from v_departamentos_individual
    //    where posicion >= @POSICION AND POSICION<(@POSICION+2)
    //GO

    //CREATE PROCEDURE SP_GRUPO_EMPLEADOS
    //(@POSICION INT)
    //AS

    //    select* from V_GRUPO_EMPLEADOS WHERE posicion>=@POSICION and posicion<(@POSICION+3)
    //GO

    #endregion
    public class HospitalRepository
    {
        private HospitalContext context;

        public HospitalRepository(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync
            (int idDepartamento)
        {
            var empleados = this.context.Empleados
                .Where(x => x.IdDepartamento == idDepartamento);
            if (empleados.Count() == 0)
            {
                return null;
            }
            else
            {
                return await empleados.ToListAsync();
            }
        }

        public async Task<int> GetNumeroRegistrosVistaDepartamentos()
        {
            return await this.context.VistaDepartamentos.CountAsync();
        }
        public async Task<int> GetNumeroRegistrosEmpleados()
        {
            return await this.context.Empleados.CountAsync();
        }

        public async Task<VistaDepartamento>
            GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await
                this.context.VistaDepartamentos
                .Where(z => z.Posicion == posicion).FirstOrDefaultAsync();
            return vista;
        }

        public async Task<List<VistaDepartamento>>
            GetGrupoVistaDepartamentoAsync(int posicion)
        {
            //SELECT* FROM V_DEPARTAMENTOS_INDIVIDUAL
            //WHERE POSICION >= 1 AND POSICION< (1 +2)
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Departamento>> GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @posicion";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);

            var consulta = this.context.Departamentos.FromSqlRaw(sql, pamPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @posicion";
            SqlParameter pamPosi = new SqlParameter("@posicion", posicion);
            var consulta=this.context.Empleados.FromSqlRaw(sql,pamPosi);
            return await consulta.ToListAsync();
        }
    }
}
