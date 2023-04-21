
using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IEmpleadoRepositorio: IRepositorio<Empleado>
    {
        //Metodo.
        void Actualizar(Empleado empleado);
    }
}