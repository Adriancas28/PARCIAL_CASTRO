
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARCIAL_CASTRO.Models
{
    [Table("Transacciones")]
    public class Transacciones
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string? NombreRem { get; set; }

        public string? NombreDes { get; set; }

        public string? PaisOri { get; set; }

        public string? PaisDest { get; set; }

        public Decimal MontoEnv { get; set; }

        public string? TipTrans { get; set; }

        public Decimal TasaCam { get; set; }

        public Decimal MontoFin { get; set; }

        public string? Estado { get; set; }

    }
}