using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("ventas")]
    public class Venta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("numero_venta")]
        public string NumeroVenta { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public Cliente? Cliente { get; set; }

        [Column("subtotal", TypeName = "numeric(10,2)")]
        public decimal Subtotal { get; set; }

        [Column("descuento", TypeName = "numeric(10,2)")]
        public decimal Descuento { get; set; }

        [Column("total", TypeName = "numeric(10,2)")]
        public decimal Total { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        public ICollection<DetalleVenta>? Detalles { get; set; }
    }
}