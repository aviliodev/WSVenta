using WSVenta.Models.Request;
using WSVenta.Models.Response;

namespace WSVenta.Servicios
{
    public interface IUserService
    {
        UserRespuesta respAut(AutenticacionRequest model);
    }
}
