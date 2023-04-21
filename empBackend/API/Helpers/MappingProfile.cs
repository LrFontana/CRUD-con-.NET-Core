

using AutoMapper;
using Core.Dto;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Compania, CompaniaDto>().ReverseMap(); // mapeo compania a companiadto

            CreateMap<Empleado, EmpleadoUpsertDto>().ReverseMap(); //mapeo de empleado a empleadoUpsertDto.
            
            CreateMap<Empleado, EmpleadoReadDto>().ForMember(e => e.CompaniaNombre, m => m.MapFrom(c => c.Compania.NombreCompania)); // mapeo de empleado con empleadoReadDto.
        }
    }
}