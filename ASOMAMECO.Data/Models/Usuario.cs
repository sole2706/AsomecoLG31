using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuarioName { get; set; }

        [Required]
        [StringLength(50)]
        public string Contrasenia { get; set; } 
      
        public string Rol { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }    
        
    }
}
