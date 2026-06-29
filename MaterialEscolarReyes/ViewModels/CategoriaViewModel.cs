using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MaterialEscolarReyes.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public IFormFile? Imagen { get; set; }

        public string? ImagenActual { get; set; }
    }
}