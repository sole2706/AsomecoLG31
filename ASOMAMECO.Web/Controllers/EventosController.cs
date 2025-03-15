using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Data.Models;
using ASOMAMECO.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASOMAMECO.Web.Controllers
{
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;
        private readonly ILogger<EventosController> _logger;

        public EventosController(
            IEventoService eventoService,
            ILogger<EventosController> logger)
        {
            _eventoService = eventoService;
            _logger = logger;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventos = await _eventoService.GetAllEventosAsync();

            EventoViewModel eventoViewModel = new EventoViewModel();
            eventoViewModel.Eventos = eventos;

            return View(eventoViewModel);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            EventoViewModel eventoViewModel = new EventoViewModel();
            eventoViewModel.Evento = evento;

            return View(eventoViewModel);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            var evento = new EventoDto
            {
                Fecha = DateTime.Today
            };

            EventoViewModel eventoViewModel = new EventoViewModel();
            eventoViewModel.Evento = evento;


            return View(eventoViewModel);
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoViewModel eventoViewModel)
        {
            EventoDto eventoDto = new EventoDto();
            eventoDto = eventoViewModel.Evento;
            if (ModelState.IsValid)
            {
                try
                {
                    await _eventoService.CreateEventoAsync(eventoDto);
                    TempData["SuccessMessage"] = "Evento creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error al crear evento");
                }
            }

            return View(eventoDto);
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            EventoViewModel eventoViewModel = new EventoViewModel();
            eventoViewModel.Evento = evento;


            return View(eventoViewModel);
        }

        // POST: Eventos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventoViewModel eventoViewModel)
        {
            var eventoDto = eventoViewModel.Evento;


            if (id != eventoDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _eventoService.UpdateEventoAsync(eventoDto);
                    TempData["SuccessMessage"] = "Evento actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error al actualizar evento");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            EventoViewModel eventoViewModel = new EventoViewModel();
            eventoViewModel.Evento = evento;

            return View(eventoViewModel);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _eventoService.DeleteEventoAsync(id);
                TempData["SuccessMessage"] = "Evento eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Error al eliminar evento");
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}