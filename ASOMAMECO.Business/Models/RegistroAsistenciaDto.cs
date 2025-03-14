using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Models
{
    public class RegistroAsistenciaDto
    {
        [Required(ErrorMessage = "El evento es requerido")]
        public int EventoId { get; set; }

        [Display(Name = "Número de Asociado")]
        public string NumeroAsociado { get; set; }

        [Display(Name = "Cédula")]
        public string Cedula { get; set; }
    }
}
