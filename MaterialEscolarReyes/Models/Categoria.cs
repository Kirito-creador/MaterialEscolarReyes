using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialEscolarReyes.Models
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Column("imagen")]
        public string? Imagen { get; set; }

        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}