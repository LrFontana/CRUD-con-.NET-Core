using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class CompaniaConfig : IEntityTypeConfiguration<Compania>
    {
        public void Configure(EntityTypeBuilder<Compania> builder)
        {
            // TODO Propiedades.

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.NombreCompania).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Direccion).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Telefono).IsRequired().HasMaxLength(30);
            builder.Property(c => c.Telefono2).IsRequired(false).HasMaxLength(30);
        }
    }
}