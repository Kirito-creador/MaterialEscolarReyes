using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("carrito")]
    public class Carrito
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}