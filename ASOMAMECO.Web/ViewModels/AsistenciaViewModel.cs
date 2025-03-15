using ASOMAMECO.Business.Models;

namespace ASOMAMECO.Web.ViewModels
{
    public class AsistenciaViewModel
    {
        public int EventoId { get; set; }
        public string? EventoNombre { get; set; }
        public string? EventoFecha { get; set; }
        public string? SearchTerm { get; set; }
        public RegistroAsistenciaDto? Registro { get; set; }
        public EventoDto? Evento { get; set; }
        public MiembroDto? Miembro { get; set; }
        public IEnumerable<AsistenciaDto>? Asistencias { get; set; }
        public int TotalAsistentes { get; set; }
        public ReporteAsistenciaDto? Reporte { get; set; }
    }
}
