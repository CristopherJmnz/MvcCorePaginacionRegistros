namespace MvcCorePaginacionRegistros.Models
{
    public class ModelPaginacionEmpleados
    {
        public int NumeroRegistros { get; set; }
        public List<Empleado> Empleados { get; set; }
        public ModelPaginacionEmpleados()
        {
            this.Empleados=new List<Empleado>();
        }
    }
}
