using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Models
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del evento es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder 100 caracteres")]
        [Display(Name = "Nombre del Evento")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha del evento es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Evento")]
        public DateTime Fecha { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no debe exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        // Cantidad de asistentes (solo para mostrar en listas)
        [Display(Name = "Asistentes")]
        public int CantidadAsistentes { get; set; }
    }
}
