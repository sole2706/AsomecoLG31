using ASOMAMECO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Interfaces
{
    public interface IEventoService
    {
        Task<IEnumerable<EventoDto>> GetAllEventosAsync();
        Task<EventoDto> GetEventoByIdAsync(int id);
        Task<EventoDto> GetEventoActivoByFechaAsync(DateTime fecha);
        Task<EventoDto> CreateEventoAsync(EventoDto eventoDto);
        Task<EventoDto> UpdateEventoAsync(EventoDto eventoDto);
        Task DeleteEventoAsync(int id);
    }
}
