
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class UnidadDeTrabajo : IUnidadDeTrabajo
    {
        //Variable
        private readonly ApplicationDbContext _db;
        public ICompaniaRepositorio Compania {get; private set;}
        public IEmpleadoRepositorio Empleado {get; private set;}
        
        //Constructor.
        public UnidadDeTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Compania = new CompaniaRepositorio(db);
            Empleado = new EmpleadoRepositorio(db);
            
        }        

        //Libera lo que no utiliza.
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync(); 
        }
    }
}