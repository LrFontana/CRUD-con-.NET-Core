
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class EmpleadoRepositorio : Repositorio<Empleado>, IEmpleadoRepositorio
    {
        //Variable.
        public ApplicationDbContext _db;

        //Constructor.        
        public EmpleadoRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
            
        }
        public void Actualizar(Empleado empleado)
        {
            //Query.
            var queryActualizarEmpleado = _db.TblEmpleado.FirstOrDefault(c => c.Id == empleado.Id);
            
            //Verifica si query es diferente a null, y si lo es actualiza los datos y guarda los cambios.
            if(queryActualizarEmpleado != null)
            {
                queryActualizarEmpleado.Nombre = empleado.Nombre;
                queryActualizarEmpleado.Apellido = empleado.Apellido;
                queryActualizarEmpleado.Direccion = empleado.Direccion;
                queryActualizarEmpleado.Cargo = empleado.Cargo;
                queryActualizarEmpleado.CompaniaId = empleado.CompaniaId;
                _db.SaveChanges();
            }
        }
    }
}