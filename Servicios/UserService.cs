using WSVenta.Models;
using WSVenta.Models.Response;
using WSVenta.Models.Request;
using WSVenta.Herramientas;
using WSVenta.Models.Common;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace WSVenta.Servicios
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public UserRespuesta respAut(AutenticacionRequest modelo)
        {
            UserRespuesta userresponse = new UserRespuesta();
            using (var db = new VentaRealContext())
            {
                string spassword = Encriptar.GetSHA256(modelo.Password);

                var usuario = db.Usuarios.Where(d => d.Email == modelo.Email && 
                                                     d.Password == spassword).FirstOrDefault();

                if (usuario == null) return null;

                userresponse.Email = usuario.Email;
                userresponse.Token = GetToken(usuario);

                
            }

            return userresponse;
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
