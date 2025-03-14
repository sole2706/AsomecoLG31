using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ASOMAMECO.Business.Models
{


    public class UsuarioDto
    {
        
        public int Id { get; set; }


        // El nombre de usuario es requerido y no debe exceder los 50 caracteres
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no debe exceder 50 caracteres")]
        public string UsuarioName { get; set; }

        // La contraseña es requerida y no debe exceder los 50 caracteres
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(50, ErrorMessage = "La contraseña no debe exceder 50 caracteres")]
        public string Contrasenia { get; set; }

        // El rol del usuario (puede ser Admin, Delegado, etc.)
        public string Rol { get; set; }

        // El email es opcional, pero debe cumplir con el formato de email
        [StringLength(100, ErrorMessage = "El correo electrónico no debe exceder 100 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string Email { get; set; }
    }
    
}


