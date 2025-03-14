using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
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
    public class MiembrosTest
    {
        private readonly Mock<IMiembroService> _mockMiembroService;
        private readonly Mock<ILogger<MiembrosController>> _mockLogger;
        private readonly MiembrosController _controller;

        public MiembrosTest()
        {
            _mockMiembroService = new Mock<IMiembroService>();
            _mockLogger = new Mock<ILogger<MiembrosController>>();
            _controller = new MiembrosController(_mockMiembroService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task MiembroExistente()
        {
            // Arrange
            // Lista de miembros "reales"
            var miembrosReales = new List<MiembroDto>
            {
                new MiembroDto { Id = 111, Nombre = "GONZALEZ QUIROS PAOLA DANIELA" }
            };

            // Configurar el mock para que consulte la lista
            _mockMiembroService.Setup(service => service.GetMiembroByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => miembrosReales.FirstOrDefault(m => m.Id == id));

            // Act
            var result = await _controller.Details(111);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MiembroViewModel>(viewResult.Model);
            Assert.Equal("GONZALEZ QUIROS PAOLA DANIELA", model.Miembro.Nombre);
        }

        [Fact]
        public async Task MiembroInexistente()
        {
            // Arrange
            // Lista de miembros "reales"
            var miembrosReales = new List<MiembroDto>
            {
                new MiembroDto { Id = 111, Nombre = "GONZALEZ QUIROS PAOLA DANIELA" }
            };

            // Configurar el mock para que consulte la lista
            _mockMiembroService.Setup(service => service.GetMiembroByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => miembrosReales.FirstOrDefault(m => m.Id == id));

            // Act
            int idInexistente = 999;
            var result = await _controller.Details(idInexistente);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
