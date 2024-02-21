using APIPeliculas;
using APIPeliculas.EndPoints;
using APIPeliculas.Entidades;
using APIPeliculas.Migrations;
using APIPeliculas.Repositorio;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

var origenesDePeticiones = builder.Configuration.GetValue<string>("origenesDePeticionesPermitidas")!; //! IGNORA ADVERTENCIA DE LOS NULOS

//INICIO AREA DE SERVICIOS 
//CONEXION A LA DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection")); 

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion => //CONFIGURA POLITICA DE CORS
    {
        //configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); //TODOS LOS ORIGENES
        configuracion.WithOrigins(origenesDePeticiones).AllowAnyHeader().AllowAnyMethod();  //ORIGENES EN ESPECIFICO
    });
    opciones.AddPolicy("PolicyFree", configuracion => 
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//SERVICIO DE SWAGGER PARA LISTAR LOS ENDPOINTS DISPONIBLES
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();

builder.Services.AddAutoMapper(typeof(Program)); // AYUDA A MAPEAR ENTIDADES

//FIN AREA DE SERVICIOS
var app = builder.Build();

//INICIO AREA MIDDLEWARE


if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseCors();

app.MapGet("/", [EnableCors(policyName: "PolicyFree")] () => "Hola mundo Aldo!"); //PRIMER ENDPOITN TEST

var endPoints = app.MapGroup("/generos"); 

app.MapGroup("/generos").MapGeneros(); //GRUPO DE ENDPOINTS


//FIN AREA MIDDLEWARE
app.Run();




