using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Business.Services;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using ASOMAMECO.Web.Controllers;
using ASOMAMECO.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Tests
{
    public class EventosTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEventoRepository> _mockEventoRepository;
        private readonly Mock<IAsistenciaRepository> _mockAsistenciaRepository;
        private readonly Mock<ILogger<EventoService>> _mockLogger;
        private readonly EventoService _service;

        public EventosTest()
        {
            _mockEventoRepository = new Mock<IEventoRepository>();
            _mockAsistenciaRepository = new Mock<IAsistenciaRepository>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(u => u.Eventos).Returns(_mockEventoRepository.Object);
            _mockUnitOfWork.Setup(u => u.Asistencias).Returns(_mockAsistenciaRepository.Object);

            _mockLogger = new Mock<ILogger<EventoService>>();

            _service = new EventoService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateEvento()
        {
            // Arrange
            var fecha = DateTime.Today;
            var eventoDto = new EventoDto
            {
                Nombre = "Evento de prueba",
                Fecha = fecha,
                Descripcion = "Descripción del evento de prueba",
                Activo = true
            };

            // No hay eventos activos para esta fecha
            _mockEventoRepository
                .Setup(repo => repo.GetEventosByFechaAsync(fecha))
                .ReturnsAsync(new List<Evento>());

            // Act
            var result = await _service.CreateEventoAsync(eventoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventoDto.Nombre, result.Nombre);
            Assert.Equal(eventoDto.Fecha, result.Fecha);
            Assert.Equal(eventoDto.Descripcion, result.Descripcion);
            Assert.Equal(eventoDto.Activo, result.Activo);

            _mockEventoRepository.Verify(repo => repo.AddAsync(It.IsAny<Evento>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

    }
}
