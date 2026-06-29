using System.ComponentModel.DataAnnotations;

namespace MaterialEscolarReyes.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string Direccion { get; set; } = "";

        [Required]
        public string Telefono { get; set; } = "";

        [Required]
        public string MetodoPago { get; set; } = "";

        public decimal Total { get; set; }
    }
}