using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Models
{
    public class Miembro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        public bool Activo { get; set; }
        public bool Estado { get; set; }

        // Relación con Asistencias
        public virtual ICollection<Asistencia> Asistencias { get; set; }
    }
}
