using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("detalle_pedidos")]
    public class DetallePedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }
    }
}