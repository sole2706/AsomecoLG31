using ASOMAMECO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Interfaces
{
    public interface IMiembroService
    {
        Task<IEnumerable<MiembroDto>> GetAllMiembrosAsync();
        Task<MiembroDto> GetMiembroByIdAsync(int id);
        Task<MiembroDto> GetMiembroByCedulaAsync(string cedula);
        Task<MiembroDto> GetMiembroByNumeroAsociadoAsync(string numeroAsociado);
        Task<MiembroDto> CreateMiembroAsync(MiembroDto miembroDto);
        Task<MiembroDto> UpdateMiembroAsync(MiembroDto miembroDto);
        Task DeleteMiembroAsync(int id);
        Task<bool> ImportarMiembrosDesdeExcelAsync(string filePath);
    }
}
