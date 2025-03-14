using ASOMAMECO.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Interfaces
{
    public interface IMiembroRepository : IGenericRepository<Miembro>
    {
        Task<Miembro> GetByCedulaAsync(string cedula);
        Task<Miembro> GetByNumeroAsociadoAsync(string numeroAsociado);
        Task<bool> ExistsCedulaAsync(string cedula);
        Task<bool> ExistsNumeroAsociadoAsync(string numeroAsociado);
    }
}
