using WSVenta.Servicios;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using WSVenta.Models.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

//Avilio: esta parte es para poder leer la seccion de AppSettings del appsettings.json, en donde definimos un token para ser usado por la api
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

//Avilio:jason web token jwt, paracrear el jwt hayq ue instalar primero la libreria Microsoft.AspNetCore.uthentication.JwtBearer
// con todo lo de abajo, el MVC sabrá que nos vamos a autenticar con un json web token
var appSettings = appSettingsSection.Get<AppSettings>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);
builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(d =>
    {
        d.RequireHttpsMetadata = false;
        d.SaveToken = true;
        d.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(llave),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/*Avilio:esta linea de abajo sirve para hacer injección de dependencia de nuestro servicio y su correspondiente interfaz
  AddScoped es para inyectar por cada request y no a toda la aplicación (como un patrón singleton).*/
builder.Services.AddScoped<IUserService, UserService>();


//Avilio: agregado para habilitar cors y conección cruzada, para que ANGULAR pueda conectarse al servicio localhost
builder.Services.AddCors(); 

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Avilio: agregado para habilitar cors y conección cruzada, para que ANGULAR pueda conectarse al servicio localhost
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
//Avilio: agregado para usar la autenticación por json web token
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
