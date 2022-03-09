using Microsoft.AspNetCore.Mvc;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Servicios;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUserService _userService;

        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AutenticacionRequest model)
        {
            Respuesta respuesta = new Respuesta();
            var userresponse = _userService.respAut(model);

            if (userresponse == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Wrong user or password";
                return BadRequest(respuesta);
            }

            respuesta.Exito = 1;
            respuesta.Data = userresponse;

            return Ok(respuesta);
        }
    }
}
