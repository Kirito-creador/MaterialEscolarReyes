using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MaterialEscolarReyes.ViewModels
{
    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public int CategoriaId { get; set; }

        public int MarcaId { get; set; }

        public int ProveedorId { get; set; }

        public IFormFile? ImagenPrincipal { get; set; }

        public IFormFile? ImagenSecundaria { get; set; }

        public string? ImagenPrincipalActual { get; set; }

        public string? ImagenSecundariaActual { get; set; }
    }
}