using System.ComponentModel.DataAnnotations;

namespace WSVenta.Models.Request
{
    public class AutenticacionRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
