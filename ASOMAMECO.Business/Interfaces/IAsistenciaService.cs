using ASOMAMECO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Interfaces
{
    public interface IAsistenciaService
    {
        Task<IEnumerable<AsistenciaDto>> GetAsistenciasByEventoIdAsync(int eventoId);
        Task<AsistenciaDto> RegistrarAsistenciaAsync(RegistroAsistenciaDto registroDto);
        Task<ReporteAsistenciaDto> GenerarReporteAsistenciaAsync(int eventoId);
    }
}
