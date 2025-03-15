using ASOMAMECO.Business.Models;

namespace ASOMAMECO.Web.ViewModels
{
    public class EventoViewModel
    {
        public EventoDto? Evento { get; set; }
        public IEnumerable<EventoDto>? Eventos { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? FechaFiltro { get; set; }
        public DateTime FechaBusqueda { get; set; } = DateTime.Today;
    }
}
