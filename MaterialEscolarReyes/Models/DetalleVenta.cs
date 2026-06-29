using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("detalle_ventas")]
    public class DetalleVenta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("venta_id")]
        public int VentaId { get; set; }

        [ForeignKey(nameof(VentaId))]
        public Venta? Venta { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [ForeignKey(nameof(ProductoId))]
        public Producto? Producto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario", TypeName = "numeric(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column("subtotal", TypeName = "numeric(10,2)")]
        public decimal Subtotal { get; set; }
    }
}