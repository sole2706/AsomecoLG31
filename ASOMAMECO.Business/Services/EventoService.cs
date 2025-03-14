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
    public class EventoService : IEventoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EventoService> _logger;

        public EventoService(IUnitOfWork unitOfWork, ILogger<EventoService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<EventoDto>> GetAllEventosAsync()
        {
            var eventos = await _unitOfWork.Eventos.GetAllAsync();
            var result = new List<EventoDto>();

            foreach (var evento in eventos)
            {
                var dto = MapToDto(evento);

                // Obtener cantidad de asistentes
                var asistencias = await _unitOfWork.Asistencias.GetAsistenciasByEventoIdAsync(evento.Id);
                dto.CantidadAsistentes = asistencias.Count();

                result.Add(dto);
            }

            return result;
        }

        public async Task<EventoDto> GetEventoByIdAsync(int id)
        {
            var evento = await _unitOfWork.Eventos.GetByIdAsync(id);
            if (evento == null) return null;

            var dto = MapToDto(evento);

            // Obtener cantidad de asistentes
            var asistencias = await _unitOfWork.Asistencias.GetAsistenciasByEventoIdAsync(evento.Id);
            dto.CantidadAsistentes = asistencias.Count();

            return dto;
        }

        public async Task<EventoDto> GetEventoActivoByFechaAsync(DateTime fecha)
        {
            var evento = await _unitOfWork.Eventos.GetEventoActivoByFechaAsync(fecha);
            if (evento == null) return null;

            var dto = MapToDto(evento);

            // Obtener cantidad de asistentes
            var asistencias = await _unitOfWork.Asistencias.GetAsistenciasByEventoIdAsync(evento.Id);
            dto.CantidadAsistentes = asistencias.Count();

            return dto;
        }

        public async Task<EventoDto> CreateEventoAsync(EventoDto eventoDto)
        {
            // Verificar si ya existe un evento activo para la fecha
            var eventosExistentes = await _unitOfWork.Eventos.GetEventosByFechaAsync(eventoDto.Fecha);
            var eventoActivoExistente = eventosExistentes.FirstOrDefault(e => e.Activo);
            if (eventoActivoExistente != null && eventoDto.Activo)
            {
                throw new InvalidOperationException($"Ya existe un evento activo para la fecha {eventoDto.Fecha.ToShortDateString()}: {eventoActivoExistente.Nombre}");
            }

            var evento = MapToEntity(eventoDto);
            await _unitOfWork.Eventos.AddAsync(evento);
            await _unitOfWork.CompleteAsync();

            return MapToDto(evento);
        }

        public async Task<EventoDto> UpdateEventoAsync(EventoDto eventoDto)
        {
            var evento = await _unitOfWork.Eventos.GetByIdAsync(eventoDto.Id);
            if (evento == null)
            {
                throw new KeyNotFoundException($"No se encontró un evento con ID {eventoDto.Id}");
            }

            // Si estamos activando este evento, verificar que no haya otro activo para la misma fecha
            if (eventoDto.Activo && !evento.Activo)
            {
                var eventosExistentes = await _unitOfWork.Eventos.GetEventosByFechaAsync(eventoDto.Fecha);
                var otroEventoActivo = eventosExistentes.FirstOrDefault(e => e.Activo && e.Id != eventoDto.Id);

                if (otroEventoActivo != null)
                {
                    throw new InvalidOperationException($"Ya existe otro evento activo para la fecha {eventoDto.Fecha.ToShortDateString()}: {otroEventoActivo.Nombre}");
                }
            }

            // Actualizar propiedades
            evento.Nombre = eventoDto.Nombre;
            evento.Fecha = eventoDto.Fecha;
            evento.Descripcion = eventoDto.Descripcion;
            evento.Activo = eventoDto.Activo;

            _unitOfWork.Eventos.Update(evento);
            await _unitOfWork.CompleteAsync();

            return MapToDto(evento);
        }

        public async Task DeleteEventoAsync(int id)
        {
            var evento = await _unitOfWork.Eventos.GetByIdAsync(id);
            if (evento == null)
            {
                throw new KeyNotFoundException($"No se encontró un evento con ID {id}");
            }

            // Verificar si tiene asistencias registradas
            var asistencias = await _unitOfWork.Asistencias.GetAsistenciasByEventoIdAsync(id);
            if (asistencias.Any())
            {
                throw new InvalidOperationException($"No se puede eliminar el evento porque tiene {asistencias.Count()} asistencias registradas");
            }

            _unitOfWork.Eventos.Remove(evento);
            await _unitOfWork.CompleteAsync();
        }

        // Métodos de mapeo
        private EventoDto MapToDto(Evento evento)
        {
            return new EventoDto
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                Fecha = evento.Fecha,
                Descripcion = evento.Descripcion,
                Activo = evento.Activo,
                CantidadAsistentes = 0 // Se llena después
            };
        }

        private Evento MapToEntity(EventoDto dto)
        {
            return new Evento
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Fecha = dto.Fecha,
                Descripcion = dto.Descripcion,
                Activo = dto.Activo
            };
        }
    }
}
