using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Models
{
    public class Asistencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MiembroId { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Required]
        public DateTime FechaHoraRegistro { get; set; }

        // Relaciones
        [ForeignKey("MiembroId")]
        public virtual Miembro Miembro { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }
    }
}
