using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASOMAMECO.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventoService _eventoService;
        private readonly IMiembroService _miembroService;

        public HomeController(
            ILogger<HomeController> logger,
            IEventoService eventoService,
            IMiembroService miembroService)
        {
            _logger = logger;
            _eventoService = eventoService;
            _miembroService = miembroService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                EventoActual = await _eventoService.GetEventoActivoByFechaAsync(DateTime.Today)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Error/{statusCode}")]
        public IActionResult StatusCodeHandler(int statusCode)
        {
            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode
            };

            switch (statusCode)
            {
                case 404:
                    viewModel.Message = "Lo sentimos, la página que busca no existe.";
                    break;
                case 500:
                    viewModel.Message = "Ha ocurrido un error interno en el servidor.";
                    break;
                default:
                    viewModel.Message = "Se ha producido un error.";
                    break;
            }

            return View("Error", viewModel);
        }
    }
}
