using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface ICompaniaRepositorio: IRepositorio<Compania>
    {
        //Metodos.
        void Actualizar(Compania compania);
    }
}