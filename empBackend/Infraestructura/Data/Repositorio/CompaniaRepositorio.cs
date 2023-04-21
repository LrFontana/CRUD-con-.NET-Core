using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class CompaniaRepositorio : Repositorio<Compania>, ICompaniaRepositorio
    {
        //Variable.
        private readonly ApplicationDbContext _db;

        //Constructor.
        public CompaniaRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
            
        }

        //Metodos.
        public void Actualizar(Compania compania)
        {
            //Query.
            var queryActualizarCompania = _db.TblCompania.FirstOrDefault(c=>c.Id == compania.Id);
            
            //Verifica si query es diferente a null, y si lo es actualiza los datos y guarda los cambios.
            if(queryActualizarCompania != null)
            {
                queryActualizarCompania.NombreCompania = compania.NombreCompania;
                queryActualizarCompania.Direccion = compania.Direccion;
                queryActualizarCompania.Telefono = compania.Telefono;
                queryActualizarCompania.Telefono2 = compania.Telefono2;
                _db.SaveChanges();
            }
        }
    }
}