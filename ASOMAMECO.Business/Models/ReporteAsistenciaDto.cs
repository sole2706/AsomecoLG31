using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Models
{
    public class ReporteAsistenciaDto
    {
        public EventoDto Evento { get; set; }
        public int TotalMiembros { get; set; }
        public int TotalAsistentes { get; set; }
        public double PorcentajeAsistencia { get; set; }
        public IEnumerable<AsistenciaDto> Asistencias { get; set; }
    }
}
