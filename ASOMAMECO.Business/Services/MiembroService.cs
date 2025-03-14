using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using ASOMAMECO.Data.Repositories;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Services
{
    public class MiembroService : IMiembroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MiembroService> _logger;

        public MiembroService(IUnitOfWork unitOfWork, ILogger<MiembroService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<MiembroDto>> GetAllMiembrosAsync()
        {
            var miembros = await _unitOfWork.Miembros.GetAllAsync();
            return miembros.Select(MapToDto);
        }

        public async Task<MiembroDto> GetMiembroByIdAsync(int id)
        {
            var miembro = await _unitOfWork.Miembros.GetByIdAsync(id);
            return miembro != null ? MapToDto(miembro) : null;
        }

        public async Task<MiembroDto> GetMiembroByCedulaAsync(string cedula)
        {
            var miembro = await _unitOfWork.Miembros.GetByCedulaAsync(cedula);
            return miembro != null ? MapToDto(miembro) : null;
        }

        public async Task<MiembroDto> GetMiembroByNumeroAsociadoAsync(string Id)
        {
            var miembro = await _unitOfWork.Miembros.GetByNumeroAsociadoAsync(Id);
            return miembro != null ? MapToDto(miembro) : null;
        }

        public async Task<MiembroDto> CreateMiembroAsync(MiembroDto miembroDto)
        {
            // Validar que no exista otro miembro con la misma cédula o número de asociado
            if (await _unitOfWork.Miembros.ExistsCedulaAsync(miembroDto.Cedula))
            {
                throw new InvalidOperationException($"Ya existe un miembro con la cédula {miembroDto.Cedula}");
            }

            if (await _unitOfWork.Miembros.ExistsNumeroAsociadoAsync(miembroDto.Id.ToString()))
            {
                throw new InvalidOperationException($"Ya existe un miembro con el número de asociado {miembroDto.Id}");
            }

            var miembro = MapToEntity(miembroDto);
            await _unitOfWork.Miembros.AddAsync(miembro);
            await _unitOfWork.CompleteAsync();

            return MapToDto(miembro);
        }

        public async Task<MiembroDto> UpdateMiembroAsync(MiembroDto miembroDto)
        {
            var miembro = await _unitOfWork.Miembros.GetByIdAsync(miembroDto.Id);
            if (miembro == null)
            {
                throw new KeyNotFoundException($"No se encontró un miembro con ID {miembroDto.Id}");
            }

            // Validar que no exista otro miembro con la misma cédula o número de asociado
            var miembroByCedula = await _unitOfWork.Miembros.GetByCedulaAsync(miembroDto.Cedula);
            if (miembroByCedula != null && miembroByCedula.Id != miembroDto.Id)
            {
                throw new InvalidOperationException($"Ya existe otro miembro con la cédula {miembroDto.Cedula}");
            }

            var miembroByNumero = await _unitOfWork.Miembros.GetByIdAsync(miembroDto.Id);
            if (miembroByNumero != null && miembroByNumero.Id != miembroDto.Id)
            {
                throw new InvalidOperationException($"Ya existe otro miembro con el número de asociado {miembroDto.Id}");
            }

            // Actualizar propiedades
            miembro.Id = miembroDto.Id;
            miembro.Cedula = miembroDto.Cedula;
            miembro.Nombre = miembroDto.Nombre;
            miembro.Email = miembroDto.Email;
            miembro.Telefono = miembroDto.Telefono;
            miembro.Activo = miembroDto.Activo;

            _unitOfWork.Miembros.Update(miembro);
            await _unitOfWork.CompleteAsync();

            return MapToDto(miembro);
        }

        public async Task DeleteMiembroAsync(int id)
        {
            var miembro = await _unitOfWork.Miembros.GetByIdAsync(id);
            if (miembro == null)
            {
                throw new KeyNotFoundException($"No se encontró un miembro con ID {id}");
            }

            // Verificar si el miembro tiene asistencias registradas
            var asistencias = await _unitOfWork.Asistencias.FindAsync(a => a.MiembroId == id);
            if (asistencias.Any())
            {
                // Mejor marcar como inactivo que eliminar físicamente si tiene asistencias
                miembro.Activo = false;
                _unitOfWork.Miembros.Update(miembro);
            }
            else
            {
                _unitOfWork.Miembros.Remove(miembro);
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ImportarMiembrosDesdeExcelAsync(string filePath)
        {
            try
            {
                _logger.LogInformation($"Iniciando importación de miembros desde Excel: {filePath}");

                var file = new FileInfo(filePath);
                if (!file.Exists)
                {
                    _logger.LogError($"El archivo no existe: {filePath}");
                    return false;
                }

                using (var package = new ExcelPackage(file))
                {
                    // Asumimos que los datos están en la primera hoja
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    // Saltar la primera fila (encabezados)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var Id = worksheet.Cells[row, 1].Value?.ToString().Trim();
                        var cedula = worksheet.Cells[row, 2].Value?.ToString().Trim();
                        var nombre = worksheet.Cells[row, 3].Value?.ToString().Trim();
                        var apellidos = worksheet.Cells[row, 4].Value?.ToString().Trim();
                        var email = worksheet.Cells[row, 5].Value?.ToString().Trim();
                        var telefono = worksheet.Cells[row, 6].Value?.ToString().Trim();

                        // Validar datos requeridos
                        if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(cedula) ||
                            string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellidos))
                        {
                            _logger.LogWarning($"Fila {row} con datos incompletos, omitiendo.");
                            continue;
                        }

                        // Verificar si ya existe
                        if (await _unitOfWork.Miembros.ExistsCedulaAsync(cedula))
                        {
                            _logger.LogInformation($"El miembro con cédula {cedula} ya existe, actualizando información.");
                            var miembroExistente = await _unitOfWork.Miembros.GetByCedulaAsync(cedula);

                            // Actualizar información
                            miembroExistente.Id = Convert.ToInt32(Id);
                            miembroExistente.Nombre = nombre;
                            miembroExistente.Email = email;
                            miembroExistente.Telefono = telefono;
                            miembroExistente.Activo = true;

                            _unitOfWork.Miembros.Update(miembroExistente);
                        }
                        else
                        {
                            // Crear nuevo miembro
                            var nuevoMiembro = new Miembro
                            {
                                Id = Convert.ToInt32(Id),
                                Cedula = cedula,
                                Nombre = nombre,
                                Email = email,
                                Telefono = telefono,
                                Activo = true
                            };

                            await _unitOfWork.Miembros.AddAsync(nuevoMiembro);
                            _logger.LogInformation($"Miembro nuevo agregado: {cedula} - {nombre} {apellidos}");
                        }
                    }
                }

                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Importación de miembros completada con éxito");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al importar miembros desde Excel");
                return false;
            }
        }

        // Métodos de mapeo
        private MiembroDto MapToDto(Miembro miembro)
        {
            return new MiembroDto
            {
                Id = miembro.Id,
                Cedula = miembro.Cedula,
                Nombre = miembro.Nombre,
                Email = miembro.Email,
                Telefono = miembro.Telefono,
                Activo = miembro.Activo
            };
        }

        private Miembro MapToEntity(MiembroDto dto)
        {
            return new Miembro
            {
                Id = dto.Id,
                Cedula = dto.Cedula,
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Activo = dto.Activo
            };
        }
    }
}
