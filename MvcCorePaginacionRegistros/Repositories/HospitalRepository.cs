using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
    //    ISNULL(EMP_NO, 0) AS EMP_NO, apellido, salario, oficio from emp
    //GO

    #endregion
    #region PROCEDURE

    //    create procedure SP_GRUPO_DEPARTAMENTOS
    //(@POSICION INT)
    //AS

    //    select* from v_departamentos_individual
    //    where posicion >= @POSICION AND POSICION<(@POSICION+2)
    //GO

    //    ALTER VIEW V_GRUPO_EMPLEADOS
    //AS

    //   select CAST(ROW_NUMBER() over (ORDER BY EMP_NO) AS INT) AS POSICION,
    //    ISNULL(EMP_NO, 0) AS EMP_NO, apellido, salario, oficio, DEPT_NO from emp
    //GO


    //    CREATE procedure SP_GRUPO_EMPLEADOS_OFICIO
    //(@POSICION INT, @oficio nvarchar(100))
    //AS
    //    select EMP_NO, apellido, oficio, salario, dept_no from(
    //    select cast(ROW_NUMBER() over (order by apellido) as int) as posicion,
    //	EMP_NO, apellido, oficio, salario, dept_no
    //    from emp
    //    where oficio=@oficio)
    //	as query
    //    where posicion >= @POSICION and posicion<(@POSICION + 2)
    //GO


    //    CREATE procedure SP_GRUPO_EMPLEADOS_OFICIO_OUT
    //(@POSICION INT, @oficio nvarchar(100), @registros int out)
    //AS
    //    SELECT @registros=count(emp_no)

    //    from emp

    //    where oficio = @oficio;

    //    select EMP_NO, apellido, oficio, salario, dept_no from(
    //    select cast(ROW_NUMBER() over (order by apellido) as int) as posicion,
    //	EMP_NO, apellido, oficio, salario, dept_no
    //    from emp
    //    where oficio=@oficio)
    //	as query
    //    where posicion >= @POSICION and posicion<(@POSICION + 2)
    //GO



//    create procedure SP_GRUPO_EMPLEADOS_DEPT
//(@posicion int, @iddept int, @registros int out)
//AS
//    select @registros=count(emp_no)

//    from emp

//    where dept_no = @iddept


//    select EMP_NO, apellido, oficio, salario, dept_no from(
//        select cast(ROW_NUMBER() over (order by apellido) as int) as posicion, 
//		EMP_NO, apellido, oficio, salario, dept_no
//    from emp
//        where DEPT_NO=@iddept
//	) as query where posicion >= @posicion and posicion<(@posicion+1)
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
        public async Task<Departamento> FindDepartamentoByIdAsync(int id)
        {
            return await 
                this.context.Departamentos
                .FirstOrDefaultAsync(x=>x.IdDepartamento==id);
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

        public async Task<int> GetNumeroRegistrosVistaDepartamentosAsync()
        {
            return await this.context.VistaDepartamentos.CountAsync();
        }
        public async Task<int> GetNumeroRegistrosEmpleadosAsync()
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

        public async Task<int> GetNumeroEmpleadosByOficio(string oficio)
        {
            return await 
                this.context.Empleados
                .Where(x => x.Oficio == oficio).CountAsync();
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosByOficioAsync(int posicion, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @posicion, @oficio";
            SqlParameter pamPosi = new SqlParameter("@posicion", posicion);
            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamPosi, pamOficio);
            return await consulta.ToListAsync();
        }


        public async Task<ModelPaginacionEmpleados> GetGrupoEmpleadosByOficioOutAsync
            (int posicion, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO_OUT @posicion, @oficio, @registros out";
            SqlParameter pamPosi = new SqlParameter("@posicion", posicion);
            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamPosi, pamOficio, pamRegistros);
            ModelPaginacionEmpleados model = new ModelPaginacionEmpleados {
                Empleados = await consulta.ToListAsync(),
                NumeroRegistros=(int) pamRegistros.Value
            };
            return model;
        }

        public async Task<ModelPaginacionDepartamentoEmpleados> GetEmpleadosByDeptAsync(int id,int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS_DEPT @posicion, @iddept, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion",posicion);
            SqlParameter pamId = new SqlParameter("@iddept", id);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta=this.context.Empleados.FromSqlRaw(sql,pamPosicion, pamId, pamRegistros);
            ModelPaginacionDepartamentoEmpleados model = new ModelPaginacionDepartamentoEmpleados
            {
                Empleado= consulta.AsEnumerable().FirstOrDefault(),
                NumeroRegistros = (int) pamRegistros.Value,
                Departamento = await this.FindDepartamentoByIdAsync(id)
            };
            return model;
        }
    }
}
