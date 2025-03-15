using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASOMAMECO.Web.Controllers
{
    public class AsistenciaController : Controller
    {
        private readonly IAsistenciaService _asistenciaService;
        private readonly IEventoService _eventoService;
        private readonly IMiembroService _miembroService;
        private readonly ILogger<AsistenciaController> _logger;

        public AsistenciaController(
            IAsistenciaService asistenciaService,
            IEventoService eventoService,
            IMiembroService miembroService,
            ILogger<AsistenciaController> logger)
        {
            _asistenciaService = asistenciaService;
            _eventoService = eventoService;
            _miembroService = miembroService;
            _logger = logger;
        }

        // GET: Asistencia/RegistrarPorEvento/5
        public async Task<IActionResult> RegistrarPorEvento(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            ViewBag.EventoId = id;
            ViewBag.EventoNombre = evento.Nombre;
            ViewBag.EventoFecha = evento.Fecha.ToString("dd/MM/yyyy");

            return View();
        }

        // POST: Asistencia/BuscarMiembro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuscarMiembro(int eventoId, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                TempData["ErrorMessage"] = "Por favor ingrese un número de cédula o número de asociado.";
                return RedirectToAction(nameof(RegistrarPorEvento), new { id = eventoId });
            }

            MiembroDto miembro = null;

            // Intentar buscar por número de asociado
            if (int.TryParse(searchTerm, out int numeroAsociado))
            {
                miembro = await _miembroService.GetMiembroByNumeroAsociadoAsync(searchTerm);
            }

            // Si no se encuentra, intentar buscar por cédula
            if (miembro == null)
            {
                miembro = await _miembroService.GetMiembroByCedulaAsync(searchTerm);
            }

            if (miembro == null)
            {
                TempData["ErrorMessage"] = "No se encontró ningún miembro con la información proporcionada.";
                return RedirectToAction(nameof(RegistrarPorEvento), new { id = eventoId });
            }

            return RedirectToAction(nameof(ConfirmarAsistencia), new { eventoId, miembroId = miembro.Id });
        }

        // GET: Asistencia/ConfirmarAsistencia
        public async Task<IActionResult> ConfirmarAsistencia(int eventoId, int miembroId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(eventoId);
            var miembro = await _miembroService.GetMiembroByIdAsync(miembroId);

            if (evento == null || miembro == null)
            {
                return NotFound();
            }

            var registroDto = new RegistroAsistenciaDto
            {
                EventoId = eventoId,
                Cedula = miembro.Cedula.ToString(),
                NumeroAsociado = miembro.Id.ToString()
            };

            return View(registroDto);
        }

        // POST: Asistencia/ConfirmarAsistencia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarAsistencia(RegistroAsistenciaDto registroDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _asistenciaService.RegistrarAsistenciaAsync(registroDto);
                    TempData["SuccessMessage"] = "Asistencia registrada correctamente.";
                    return RedirectToAction(nameof(RegistrarPorEvento), new { id = registroDto.EventoId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error al registrar asistencia");
                }
            }

            // Recargar los datos del evento y miembro en caso de error
            var evento = await _eventoService.GetEventoByIdAsync(registroDto.EventoId);
            var miembro = await _miembroService.GetMiembroByIdAsync(Convert.ToInt32(registroDto.NumeroAsociado));

            if (evento != null && miembro != null)
            {
                registroDto.NumeroAsociado = miembro.Id.ToString();
                registroDto.Cedula = miembro.Cedula;
            }

            return View(registroDto);
        }

        // GET: Asistencia/ListadoPorEvento/5
        public async Task<IActionResult> ListadoPorEvento(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            var asistencias = await _asistenciaService.GetAsistenciasByEventoIdAsync(id);

            ViewBag.EventoId = id;
            ViewBag.EventoNombre = evento.Nombre;
            ViewBag.EventoFecha = evento.Fecha.ToString("dd/MM/yyyy");
            ViewBag.TotalAsistentes = asistencias.Count();

            return View(asistencias);
        }

        // GET: Asistencia/ReporteGeneral/5
        public async Task<IActionResult> ReporteGeneral(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);
            
            if (evento == null)
            {
                return NotFound();
            }

            try
            {
                var reporte = await _asistenciaService.GenerarReporteAsistenciaAsync(id);

                ViewBag.EventoId = id;
                ViewBag.EventoNombre = evento.Nombre;
                ViewBag.EventoFecha = evento.Fecha.ToString("dd/MM/yyyy");

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Error al generar reporte de asistencia");
                return RedirectToAction("Index", "Eventos");
            }
        }
    }
}