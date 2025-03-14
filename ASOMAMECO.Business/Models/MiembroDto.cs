using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Models
{
    public class MiembroDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(20, ErrorMessage = "La cédula no debe exceder 20 caracteres")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no debe exceder 200 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no debe exceder 100 caracteres")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no debe exceder 20 caracteres")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        // Propiedad para mostrar el nombre completo
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{Nombre}";
    }
}
