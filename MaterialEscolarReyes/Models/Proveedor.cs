using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("proveedores")]
    public class Proveedor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("correo")]
        public string? Correo { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }
    }
}