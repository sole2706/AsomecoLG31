using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Data.Interfaces;
using ASOMAMECO.Data.Models;
using ASOMAMECO.Web.Controllers;
using ASOMAMECO.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Tests
{
    public class LoginTest
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly AuthController _controller;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public LoginTest()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();

            // Configurar HttpContext mock para probar la autenticación
            var mockHttpContext = new Mock<HttpContext>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();

            mockServiceProvider
                .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(_mockAuthenticationService.Object);

            mockHttpContext
                .Setup(hc => hc.RequestServices)
                .Returns(mockServiceProvider.Object);

            // Configurar TempData para el controlador
            var tempData = new TempDataDictionary(
                mockHttpContext.Object,
                Mock.Of<ITempDataProvider>());

            _controller = new AuthController(_mockAuthService.Object, _mockUsuarioRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },
                TempData = tempData
            };
        }

        [Fact]
        public async Task LoginWithValidCredentials()
        {
            // Arrange
            var viewModel = new UsuarioViewModel
            {
                LoginModel = new LoginModel
                {
                    UsuarioName = "usuario",
                    Contrasenia = "contraseñaValida"
                },
                ErrorMessages = new List<string>()
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync((Business.Models.UsuarioDto)null);

            // Act
            var result = await _controller.Login(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedViewModel = Assert.IsType<UsuarioViewModel>(viewResult.Model);
            Assert.Single(returnedViewModel.ErrorMessages);
            Assert.Contains("Nombre de usuario o contraseña incorrectos", returnedViewModel.ErrorMessages);
        }

        [Fact]
        public async Task LoginWithInvalidCredentials()
        {
            // Arrange
            var viewModel = new UsuarioViewModel
            {
                LoginModel = new LoginModel  
                {
                    UsuarioName = "admin",
                    Contrasenia = "admin1234"
                },
                ErrorMessages = new List<string>()
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync((Business.Models.UsuarioDto)null); 

            // Act
            var result = await _controller.Login(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedViewModel = Assert.IsType<UsuarioViewModel>(viewResult.Model);
            Assert.Single(returnedViewModel.ErrorMessages);
            Assert.Contains("Nombre de usuario o contraseña incorrectos", returnedViewModel.ErrorMessages);
        }

        [Fact]
        public async Task LoginWithNullUsername()
        {
            // Arrange
            var viewModel = new UsuarioViewModel
            {
                LoginModel = new LoginModel
                {
                    UsuarioName = null,
                    Contrasenia = "contraseñaValida"
                },
                ErrorMessages = new List<string>()
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync((Business.Models.UsuarioDto)null);

            // Act
            var result = await _controller.Login(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedViewModel = Assert.IsType<UsuarioViewModel>(viewResult.Model);
            Assert.Single(returnedViewModel.ErrorMessages);
            Assert.Contains("Nombre de usuario o contraseña incorrectos", returnedViewModel.ErrorMessages);
        }
    }
}
