using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }


        [Column("nombre")]
        public string Nombre { get; set; } = "";


        [Column("email")]
        public string Correo { get; set; } = "";


        [Column("password")]
        public string Password { get; set; } = "";


        [Column("rol")]
        public string Rol { get; set; } = "";


        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }
    }
}