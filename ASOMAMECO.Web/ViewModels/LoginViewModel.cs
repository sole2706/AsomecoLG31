using ASOMAMECO.Business.Models;

namespace ASOMAMECO.Web.ViewModels
{
    public class UsuarioViewModel
    {
        public UsuarioDto? Usuario { get; set; }
        public IEnumerable<UsuarioDto>? Usuarios { get; set; }
        public string? SearchTerm { get; set; }
        public bool MostrarSoloActivos { get; set; } = true;
        public LoginModel? LoginModel { get; set; }
        public bool ProcesadoCorrectamente { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }



    public class AuthResultViewModel
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public string? Token { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
    }
}