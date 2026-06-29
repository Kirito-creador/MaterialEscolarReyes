using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("pedidos")]
    public class Pedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "Pendiente";

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("metodo_pago")]
        public string? MetodoPago { get; set; }

        public ICollection<DetallePedido>? Detalles { get; set; }
    }
}