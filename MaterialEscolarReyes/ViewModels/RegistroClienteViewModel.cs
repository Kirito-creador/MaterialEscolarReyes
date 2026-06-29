using System.ComponentModel.DataAnnotations;

namespace MaterialEscolarReyes.ViewModels
{
    public class RegistroClienteViewModel
    {
        [Required]
        public string Nombre { get; set; } = "";

        [Required]
        public string Apellido { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = "";

        [Required]
        public string Telefono { get; set; } = "";

        public string? Direccion { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmarPassword { get; set; } = "";
    }
}