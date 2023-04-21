using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class EmpleadoConfig : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            //TODO Propiedades.

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Apellido).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Direccion).IsRequired().HasMaxLength(30);
            builder.Property(e => e.Cargo).IsRequired().HasMaxLength(30);
            builder.Property(e => e.CompaniaId).IsRequired();

            //TODO Relaciones.
            builder.HasOne(e => e.Compania).WithMany().HasForeignKey(e => e.CompaniaId);


        }
    }
}