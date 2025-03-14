using ASOMAMECO.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMiembroRepository Miembros { get; }
        IEventoRepository Eventos { get; }
        IAsistenciaRepository Asistencias { get; }
        Task<int> CompleteAsync();
    }
}
