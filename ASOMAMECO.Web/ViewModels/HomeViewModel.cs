using ASOMAMECO.Business.Models;

namespace ASOMAMECO.Web.ViewModels
{
    public class HomeViewModel
    {
        public EventoDto? EventoActual { get; set; }
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
