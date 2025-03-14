using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Models
{
    public class AsistenciaDto
    {
        public int Id { get; set; }

        public int MiembroId { get; set; }

        public int EventoId { get; set; }

        [Display(Name = "Fecha y Hora de Registro")]
        public DateTime FechaHoraRegistro { get; set; }

        // Propiedades de navegación
        [Display(Name = "Miembro")]
        public string NombreMiembro { get; set; }

        [Display(Name = "Número de Asociado")]
        public string NumeroAsociado { get; set; }

        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Display(Name = "Evento")]
        public string NombreEvento { get; set; }

        [Display(Name = "Fecha del Evento")]
        [DataType(DataType.Date)]
        public DateTime FechaEvento { get; set; }
    }
}
