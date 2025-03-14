using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Método para iniciar sesión
        public async Task<UsuarioDto> LoginAsync(LoginModel loginModel)
        {
            // Buscar el usuario en la base de datos por su nombre de usuario
            var usuario = await _usuarioRepository.GetByUsuarioNameAsync(loginModel.UsuarioName);

            if (usuario == null)
            {
                // Si no existe el usuario, devolver null o lanzar excepción
                return null;
            }

            // Verificar la contraseña: Comparar la contraseña directamente (sin hash)
            if (usuario.Contrasenia != loginModel.Contrasenia)
            {
                // Si la contraseña no coincide, devolver null o lanzar excepción
                return null;
            }

            // Si la validación fue exitosa, convertir el usuario a un DTO
            var usuarioDTO = new UsuarioDto
            {
                Id = usuario.Id,
                UsuarioName = usuario.UsuarioName,
                Rol = usuario.Rol,
                Email = usuario.Email
            };

            // Devolver el usuario DTO
            return usuarioDTO;
        }
    }


}
