using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Interfaces
{
    public interface IAsistenciaRepository : IGenericRepository<Asistencia>
    {
        Task<IEnumerable<Asistencia>> GetAsistenciasByEventoIdAsync(int eventoId);
        Task<bool> ExistsAsistenciaAsync(int miembroId, int eventoId);
        Task<IEnumerable<Asistencia>> GetAsistenciasByFechaAsync(DateTime fecha);
    }
}
