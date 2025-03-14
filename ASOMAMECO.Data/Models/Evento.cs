using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        // Relación con Asistencias
        public virtual ICollection<Asistencia> Asistencias { get; set; }
    }
}
