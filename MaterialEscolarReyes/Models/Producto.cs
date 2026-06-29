using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("codigo")]
        [StringLength(30)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [Column("nombre")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("precio_compra", TypeName = "numeric(10,2)")]
        public decimal PrecioCompra { get; set; }

        [Column("precio_venta", TypeName = "numeric(10,2)")]
        public decimal PrecioVenta { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("stock_minimo")]
        public int StockMinimo { get; set; }
        [Column("imagen_principal")]
        [StringLength(255)]
        public string? ImagenPrincipal { get; set; }

        [Column("imagen_secundaria")]
        [StringLength(255)]
        public string? ImagenSecundaria { get; set; }

        [Column("categoria_id")]
        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        [Column("marca_id")]
        public int MarcaId { get; set; }

        public Marca? Marca { get; set; }

        [Column("proveedor_id")]
        public int ProveedorId { get; set; }

        public Proveedor? Proveedor { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        public ICollection<DetalleVenta>? DetallesVenta { get; set; }

        public ICollection<MovimientoInventario>? MovimientosInventario { get; set; }
    }
}