using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpGet] /*protocolo a usar*/
        public IActionResult Get()
        {
            using(VentaRealContext db = new VentaRealContext())
            {
                var lst = db.Clientes.ToList();
                return Ok(lst);
            }
            
        }
    }
}
