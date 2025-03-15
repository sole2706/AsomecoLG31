using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASOMAMECO.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IAuthService authService, IUsuarioRepository usuarioRepository)
        {
            _authService = authService;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var viewModel = new UsuarioViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuarioViewModel viewModel)
        {
            if (viewModel.LoginModel == null)
            {
                viewModel.ErrorMessages.Add("Datos de inicio de sesión no proporcionados");
                return View(viewModel);
            }

            var loginModel = new LoginModel
            {
                UsuarioName = viewModel.LoginModel.UsuarioName,
                Contrasenia = viewModel.LoginModel.Contrasenia
            };

            var usuario = await _authService.LoginAsync(loginModel);

            if (usuario == null)
            {
                viewModel.ErrorMessages.Add("Nombre de usuario o contraseña incorrectos");
                return View(viewModel);
            }

            // Crear claims para la autenticación
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UsuarioName),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = viewModel.RememberMe,
                RedirectUri = viewModel.ReturnUrl
            };

           

            if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
            {
                return Redirect(viewModel.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }




        [HttpPost]
        
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Login");
        }

        
    }
}


