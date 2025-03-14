using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Data.Context;
using ASOMAMECO.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Repositories
{
    public class EventoRepository : GenericRepository<Evento>, IEventoRepository
    {
        public EventoRepository(ASOMAMECOContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Evento>> GetEventosByFechaAsync(DateTime fecha)
        {
            return await _context.Eventos
                .Where(e => e.Fecha.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<Evento> GetEventoActivoByFechaAsync(DateTime fecha)
        {
            return await _context.Eventos
                .FirstOrDefaultAsync(e => e.Fecha.Date == fecha.Date && e.Activo);
        }
    }
}
