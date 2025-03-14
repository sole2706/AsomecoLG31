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
    public class MiembroRepository : GenericRepository<Miembro>, IMiembroRepository
    {
        public MiembroRepository(ASOMAMECOContext context) : base(context)
        {
        }

        public async Task<Miembro> GetByCedulaAsync(string cedula)
        {
            return await _context.Miembros.FirstOrDefaultAsync(m => m.Cedula == cedula);
        }

        public async Task<Miembro> GetByNumeroAsociadoAsync(string numeroAsociado)
        {
            return await _context.Miembros.FirstOrDefaultAsync(m => m.Id.ToString() == numeroAsociado);
        }

        public async Task<bool> ExistsCedulaAsync(string cedula)
        {
            return await _context.Miembros.AnyAsync(m => m.Cedula == cedula);
        }

        public async Task<bool> ExistsNumeroAsociadoAsync(string numeroAsociado)
        {
            return await _context.Miembros.AnyAsync(m => m.Id.ToString() == numeroAsociado);
        }
    }
}
