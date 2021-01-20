using System.ComponentModel.DataAnnotations;

namespace FCPC.Models
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Password { get; set; }

    }
}
