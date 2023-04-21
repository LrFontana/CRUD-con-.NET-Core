using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IUnidadDeTrabajo: IDisposable //IDisposable ignora lo que no esta siendo utilizado para liberar espacio. 
    {
        ICompaniaRepositorio Compania {get; }
        IEmpleadoRepositorio Empleado {get; }

        Task Guardar();
    }
}