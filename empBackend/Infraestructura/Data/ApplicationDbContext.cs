using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Infraestructura.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Constructor.
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Relaciones con los Db (CompaniaConfig.cs, EmpleadoConfig.cs)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                        
            modelBuilder.ApplyConfiguration(new CompaniaConfig());
            modelBuilder.ApplyConfiguration(new EmpleadoConfig());
        }

        //Variables.
        public DbSet<Compania> TblCompania { get; set; }
        public DbSet<Empleado> TblEmpleado { get; set; }
    }   
}