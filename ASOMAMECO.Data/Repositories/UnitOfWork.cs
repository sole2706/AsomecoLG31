using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Data.Context;
using ASOMAMECO.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ASOMAMECOContext _context;
        public IMiembroRepository Miembros { get; private set; }
        public IEventoRepository Eventos { get; private set; }
        public IAsistenciaRepository Asistencias { get; private set; }

        public UnitOfWork(ASOMAMECOContext context)
        {
            _context = context;
            Miembros = new MiembroRepository(_context);
            Eventos = new EventoRepository(_context);
            Asistencias = new AsistenciaRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
