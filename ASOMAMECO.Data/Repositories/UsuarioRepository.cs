using ASOMAMECO.Data.Context;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ASOMAMECOContext context) : base(context)
        {
        }

        // Obtener un usuario por su email
        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Obtener un usuario por su nombre de usuario
        public async Task<Usuario> GetByUsuarioNameAsync(string usuarioName)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioName == usuarioName);
        }

        // Verificar si existe un usuario con la misma dirección de correo electrónico
        public async Task<bool> ExistsEmailAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        // Verificar si existe un usuario con el mismo nombre de usuario
        public async Task<bool> ExistsUsuarioNameAsync(string usuarioName)
        {
            return await _context.Usuarios.AnyAsync(u => u.UsuarioName == usuarioName);
        }
    }

}
