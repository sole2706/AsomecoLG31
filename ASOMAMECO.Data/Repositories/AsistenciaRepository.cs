using ASOMAMECO.Data.Context;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Repositories
{
    public class AsistenciaRepository : GenericRepository<Asistencia>, IAsistenciaRepository
    {
        public AsistenciaRepository(ASOMAMECOContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Asistencia>> GetAsistenciasByEventoIdAsync(int eventoId)
        {
            return await _context.Asistencias
                .Include(a => a.Miembro)
                .Where(a => a.EventoId == eventoId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsistenciaAsync(int miembroId, int eventoId)
        {
            return await _context.Asistencias
                .AnyAsync(a => a.MiembroId == miembroId && a.EventoId == eventoId);
        }

        public async Task<IEnumerable<Asistencia>> GetAsistenciasByFechaAsync(DateTime fecha)
        {
            return await _context.Asistencias
                .Include(a => a.Miembro)
                .Include(a => a.Evento)
                .Where(a => a.Evento.Fecha.Date == fecha.Date)
                .ToListAsync();
        }
    }
}
