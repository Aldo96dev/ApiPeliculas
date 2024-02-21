using APIPeliculas.Entidades;
using Microsoft.EntityFrameworkCore;

namespace APIPeliculas
{
    public class ApplicationDbContext: DbContext //PIEZA CENTRAL DE EFC CONFIGURA TABLAS Y REALIZAR CONSULTAS A LA DB
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {            
        }

        public DbSet<Genero> Generos { get; set; }
    }
}
