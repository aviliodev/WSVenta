using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;
using WSVenta.Models.Response;
using WSVenta.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//Al poner Authorize, no nos dejará consultar los clientes si no mandamos el token. El token se generó al hacer login.
    public class ClienteController : ControllerBase
    {
        [HttpGet] /*protocolo a usar, para especificar que entraremos por Get al método IActionResult*/
        public IActionResult Get()
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                using (VentaRealContext db = new VentaRealContext()) /*conección a la base de datos con entity framework*/
                {
                    //var lst = db.Clientes.ToList();
                    var lst = db.Clientes.OrderByDescending(x => x.Id).ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Mensaje = "Exito";
                    oRespuesta.Data = lst;
                    //return Ok(lst); /*el método Ok transform la lista en un json*/
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta); /*el método Ok transform la oRespuesta en un json*/
        }

        [HttpPost]
        public IActionResult Add(ClientRequest oModel)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {
                    oRespuesta.Exito = 1;
                    Cliente oCliente = new Cliente();
                    oCliente.Nombre = oModel.Nombre;
                    db.Clientes.Add(oCliente);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPut]
        public IActionResult Edit(ClientRequest oModel)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {

                    Cliente oCliente = db.Clientes.Find(oModel.Id);
                    oCliente.Nombre = oModel.Nombre;

                    db.Entry(oCliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();

                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpDelete("{Id}")] //Se le agrega un parametro de entrada "{Id}", para indicar que el id se mandará con la URL. eje: https://localhost:7003/api/cliente/nuestro_id
        public IActionResult Delete(int Id)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {

                    Cliente oCliente = db.Clientes.Find(Id);

                    db.Remove(oCliente);
                    db.SaveChanges();

                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
    }
}
