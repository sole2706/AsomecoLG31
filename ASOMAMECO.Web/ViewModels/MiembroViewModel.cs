using ASOMAMECO.Business.Models;

namespace ASOMAMECO.Web.ViewModels
{
    public class MiembroViewModel
    {
        public MiembroDto? Miembro { get; set; }
        public IEnumerable<MiembroDto>? Miembros { get; set; }
        public string? SearchTerm { get; set; }
        public bool MostrarSoloActivos { get; set; } = true;
        public IFormFile? ArchivoExcel { get; set; }
        public bool ProcesadoCorrectamente { get; set; }
        public int TotalRegistrosImportados { get; set; }
        public int TotalRegistrosConError { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
