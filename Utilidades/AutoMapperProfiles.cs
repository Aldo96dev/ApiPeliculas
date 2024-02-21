using APIPeliculas.DTOs;
using APIPeliculas.Entidades;
using AutoMapper;

namespace APIPeliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<CrearGeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }
    }
}
