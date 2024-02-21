using APIPeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace APIPeliculas.Repositorio
{
    public interface IRepositorioGeneros
    {
        Task<int> Crear(Genero generos); 
        Task<Genero?> ObtenerPorId(int id);
        Task<List<Genero>> ObtenerTodos();
        Task<bool> ExisteId(int id);
        Task Actualizar(Genero generos); 
        Task Borrar(int id);
        //List<Genero> spGetGeneros(int id); //Task
        Task<List<Genero>> spGetGeneros(int id);
    }
}
