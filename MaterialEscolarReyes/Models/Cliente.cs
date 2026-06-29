using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = "";

        [Column("apellido")]
        public string? Apellido { get; set; }

        [Required]
        [Column("correo")]
        public string Correo { get; set; } = "";

        [Required]
        [Column("password")]
        public string Password { get; set; } = "";

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }
    }
}