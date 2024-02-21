using APIPeliculas.Entidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace APIPeliculas.Repositorio
{ 
    public class RepositorioGeneros : IRepositorioGeneros
    {
        private readonly ApplicationDbContext context;

        public RepositorioGeneros(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Actualizar(Genero generos)
        {
            context.Update(generos);
            await context.SaveChangesAsync();

        }

        public async Task Borrar(int id)
        {
            await context.Generos.Where(x => x.Id == id).ExecuteDeleteAsync();
        }       

        public async Task<int> Crear(Genero generos)
        {
            context.Add(generos);
            await context.SaveChangesAsync();
            return generos.Id;
        }

        public async Task<bool> ExisteId(int id)
        {
            return await context.Generos.AnyAsync(x => x.Id == id); //BUSCA SI EXISTE ALGUN REGISTRO CON DICHO ID
        }

        public async Task<Genero?> ObtenerPorId(int id) //OPERADOR ELVIS A GENERO PORQUE PUEDE SER NULL
        {
            return await context.Generos.FirstOrDefaultAsync( x => x.Id == id); //PUEDE SER NULL

        }

        public async Task<List<Genero>> ObtenerTodos()
        {
            //return await context.Generos.OrderByDescending(x => x.Nombre).ToListAsync(); //METODO QUE OBTIENE TODOS LOS GENEROS
            return await context.Generos.ToListAsync(); //METODO QUE OBTIENE TODOS LOS GENEROS

        }
        public async Task<List<Genero>> spGetGeneros(int id)
        {
            try
            {
                List<Genero> listGenero = new List<Genero>();
                //var listGenero = new List<Genero>();
                SqlConnection conexion = (SqlConnection)context.Database.GetDbConnection();
                SqlCommand comando = conexion.CreateCommand();
                conexion.Open();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "spObtieneId";
                comando.Parameters.Add("@id", System.Data.SqlDbType.Int, 5).Value = 1;
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Genero listG = new Genero();
                    listG.Id = (int)reader["id"];
                    listG.Nombre = (string)reader["Nombre"];
                    listGenero.Add(listG);
                }
                conexion.Close();
                return listGenero;

            }
            catch (Exception ex)
            {
                var menssage = ex.Message;
                return null;
            }
            
        }

        public partial class EnvioEmail
        {
            private string myEmail = "aldo96dev@gmail.com";
            private string passwordEmail = "qyxlkdjtniohwef";
            private string aliasEmail = "CODIGO DE VERIFICACION";
            private string[] docAdjuntos;
            private MailMessage? mCorreo;

            


            private void CrearCuerpoCorreo()
            {
                mCorreo = new MailMessage();
                mCorreo.From = new MailAddress(myEmail, aliasEmail, System.Text.Encoding.UTF8);
                mCorreo.To.Add("aldo96ale@gmail.com".Trim());
                mCorreo.Subject = "Subject".Trim();
                mCorreo.Body = "Body".Trim();
                mCorreo.IsBodyHtml = true;
                mCorreo.Priority = MailPriority.High; 
            }

            private void Enviar()
            {
                try {
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = 25;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential(myEmail, passwordEmail);
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    { return true; };
                    smtp.EnableSsl = true;
                    smtp.Send(mCorreo);
                }
                catch (Exception ex)
                {
                    var menssage = ex.Message;
                }

                
            }

        }


    }
}