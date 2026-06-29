using System.ComponentModel.DataAnnotations;

namespace MaterialEscolarReyes.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Ingrese su correo")]
        [EmailAddress]
        public string Correo { get; set; } = "";


        [Required(ErrorMessage = "Ingrese su contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

    }
}