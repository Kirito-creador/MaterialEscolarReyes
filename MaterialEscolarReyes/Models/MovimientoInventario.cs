using System.ComponentModel.DataAnnotations;

namespace MaterialEscolarReyes.Models
{
    public class MovimientoInventario
    {
        [Key]
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string TipoMovimiento { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public string Observacion { get; set; } = string.Empty;
    }
}