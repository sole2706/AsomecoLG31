using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AsistenciaService> _logger;

        public AsistenciaService(IUnitOfWork unitOfWork, ILogger<AsistenciaService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<AsistenciaDto>> GetAsistenciasByEventoIdAsync(int eventoId)
        {
            var asistencias = await _unitOfWork.Asistencias.GetAsistenciasByEventoIdAsync(eventoId);
            return asistencias.Select(MapToDto);
        }

        public async Task<AsistenciaDto> RegistrarAsistenciaAsync(RegistroAsistenciaDto registroDto)
        {
            // Validar que el evento exista y esté activo
            var evento = await _unitOfWork.Eventos.GetByIdAsync(registroDto.EventoId);
            if (evento == null)
            {
                throw new KeyNotFoundException($"No se encontró un evento con ID {registroDto.EventoId}");
            }

            if (!evento.Activo)
            {
                throw new InvalidOperationException($"El evento '{evento.Nombre}' no está activo");
            }

            // Buscar el miembro por cédula o número de asociado
            Miembro miembro = null;
            if (!string.IsNullOrEmpty(registroDto.Cedula))
            {
                miembro = await _unitOfWork.Miembros.GetByCedulaAsync(registroDto.Cedula);
            }
            else if (!string.IsNullOrEmpty(registroDto.NumeroAsociado))
            {
                miembro = await _unitOfWork.Miembros.GetByNumeroAsociadoAsync(registroDto.NumeroAsociado);
            }

            if (miembro == null)
            {
                throw new KeyNotFoundException("No se encontró el miembro con la información proporcionada");
            }

            if (!miembro.Activo)
            {
                throw new InvalidOperationException($"El miembro {miembro.Nombre} no está activo");
            }

            // Verificar que no exista ya una asistencia para este miembro y evento
            if (await _unitOfWork.Asistencias.ExistsAsistenciaAsync(miembro.Id, evento.Id))
            {
                throw new InvalidOperationException($"El miembro {miembro.Nombre} ya tiene registrada su asistencia para este evento");
            }

            // Crear la asistencia
            var asistencia = new Asistencia
            {
                MiembroId = miembro.Id,
                EventoId = evento.Id,
                FechaHoraRegistro = DateTime.Now
            };

            await _unitOfWork.Asistencias.AddAsync(asistencia);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Asistencia registrada: Miembro {miembro.Id} - {miembro.Nombre}, Evento {evento.Id} - {evento.Nombre}");

            // Cargar las relaciones para el DTO
            asistencia.Miembro = miembro;
            asistencia.Evento = evento;

            return MapToDto(asistencia);
        }

        public async Task<ReporteAsistenciaDto> GenerarReporteAsistenciaAsync(int eventoId)
        {
            // Obtener el evento
            var evento = await _unitOfWork.Eventos.GetByIdAsync(eventoId);
            if (evento == null)
            {
                throw new KeyNotFoundException($"No se encontró un evento con ID {eventoId}");
            }

            // Obtener las asistencias del evento
            var asistencias = await _unitOfWork.Asistencias.GetAsistenciasByEventoIdAsync(eventoId);

            // Obtener el total de miembros activos
            var miembros = await _unitOfWork.Miembros.FindAsync(m => m.Activo);
            var totalMiembros = miembros.Count();
            var totalAsistentes = asistencias.Count();

            // Calcular porcentaje de asistencia
            var porcentaje = totalMiembros > 0 ? (double)totalAsistentes / totalMiembros * 100 : 0;

            var reporte = new ReporteAsistenciaDto
            {
                Evento = new EventoDto
                {
                    Id = evento.Id,
                    Nombre = evento.Nombre,
                    Fecha = evento.Fecha,
                    Descripcion = evento.Descripcion,
                    Activo = evento.Activo,
                    CantidadAsistentes = totalAsistentes
                },
                TotalMiembros = totalMiembros,
                TotalAsistentes = totalAsistentes,
                PorcentajeAsistencia = Math.Round(porcentaje, 2),
                Asistencias = asistencias.Select(MapToDto)
            };

            return reporte;
        }

        // Métodos de mapeo
        private AsistenciaDto MapToDto(Asistencia asistencia)
        {
            return new AsistenciaDto
            {
                Id = asistencia.Id,
                MiembroId = asistencia.MiembroId,
                EventoId = asistencia.EventoId,
                FechaHoraRegistro = asistencia.FechaHoraRegistro,
                NombreMiembro = asistencia.Miembro.Nombre,
                NumeroAsociado = asistencia.Miembro.Id.ToString(),
                Cedula = asistencia.Miembro.Cedula,
                NombreEvento = asistencia.Evento.Nombre,
                FechaEvento = asistencia.Evento.Fecha
            };
        }
    }
}
