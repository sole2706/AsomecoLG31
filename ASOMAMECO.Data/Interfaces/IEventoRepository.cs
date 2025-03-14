using ASOMAMECO.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Interfaces
{
    public interface IEventoRepository : IGenericRepository<Evento>
    {
        Task<IEnumerable<Evento>> GetEventosByFechaAsync(DateTime fecha);
        Task<Evento> GetEventoActivoByFechaAsync(DateTime fecha);
    }
}
