using APIPeliculas.DTOs;
using APIPeliculas.Entidades;
using APIPeliculas.Repositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Mail;

namespace APIPeliculas.EndPoints
{

    public static class GenerosEndpoints //UNA CLASE POR ENTIDAD O CLASE POR ENDPOINT
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerGeneros);
            group.MapGet("/{id:int}", ObtenerGeneroId);
            group.MapPost("/", CrearGenero);
            group.MapPut("/{id:int}", ActualizarGenero);
            group.MapDelete("/{id:int}", BorrarGenero);
            group.MapGet("/GetAllGeneros{id:int}", spGetGeneros);
            return group;
        }

        static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositorioGeneros repositorio, IMapper mapper)
        {
            var generos = await repositorio.ObtenerTodos();
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
            return TypedResults.Ok(generosDTO);
        }
        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroId(IRepositorioGeneros repositorio, int id,
            IMapper mapper)
        {
            var genero = await repositorio.ObtenerPorId(id);
            if (genero is null)
            {
                return TypedResults.NotFound();
            }
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Ok(generoDTO);
        }

        static async Task<Created<GeneroDTO>> CrearGenero(CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repositorio,
            IMapper mapper)
        {
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            var id = await repositorio.Crear(genero);
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Created($"generos/{id}", generoDTO);
        }
        static async Task<Results<NoContent, NotFound>> ActualizarGenero(int id, CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repositorio,
            IMapper mapper)
        {
            var existe = await repositorio.ExisteId(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            var generos = mapper.Map<Genero>(crearGeneroDTO);
            generos.Id = id;
            await repositorio.Actualizar(generos);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repositorio)
        {
            var existe = await repositorio.ExisteId(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            await repositorio.Borrar(id);
            return TypedResults.NoContent();
        }

        /*static List<Genero> spGetGeneros(IRepositorioGeneros repositorio, int id)
        {
            var listGeneros = repositorio.GetGeneros(id);
            return listGeneros;
        }*/

        static async Task<Ok<List<Genero>>> spGetGeneros(IRepositorioGeneros repositorio, int id)
        {
            var generos = await repositorio.spGetGeneros(id);
            //var generosDTO = List<Genero>(generos);
            //return listGeneros;
            return TypedResults.Ok(generos);
        }

        //SIN AUTOMAPPER
        /*static async Task<Created<Genero>> CrearGenero(Genero generos, IRepositorioGeneros repositorio)
        {
            var id = await repositorio.Crear(generos);
            return TypedResults.Created($"generos/{id}", generos);
        }*/


    }
}
