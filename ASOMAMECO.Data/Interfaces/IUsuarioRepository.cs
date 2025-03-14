using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> GetByUsuarioNameAsync(string usuarioName);
        Task<bool> ExistsEmailAsync(string email);
        Task<bool> ExistsUsuarioNameAsync(string usuarioName);
    }

}
