using ASOMAMECO.Business.Interfaces;
using ASOMAMECO.Business.Models;
using ASOMAMECO.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASOMAMECO.Web.Controllers
{
    public class MiembrosController : Controller
    {
        private readonly IMiembroService _miembroService;
        private readonly ILogger<MiembrosController> _logger;

        public MiembrosController(
            IMiembroService miembroService,
            ILogger<MiembrosController> logger)
        {
            _miembroService = miembroService;
            _logger = logger;
        }

        // GET: Miembros
        public async Task<IActionResult> Index()
        {
            var miembros = await _miembroService.GetAllMiembrosAsync();

            MiembroViewModel miembroViewModel = new MiembroViewModel();
            miembroViewModel.Miembros = miembros;

            return View(miembroViewModel);
        }

        // GET: Miembros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var miembro = await _miembroService.GetMiembroByIdAsync(id);

            if (miembro == null)
            {
                return NotFound();
            }

            MiembroViewModel miembroViewModel = new MiembroViewModel();
            miembroViewModel.Miembro = miembro;
            return View(miembroViewModel);
        }

        // GET: Miembros/Create
        public IActionResult Create()
        {
            var miembro = new MiembroDto
            {
                Activo = true
            };

            MiembroViewModel miembroViewModel = new MiembroViewModel();
            miembroViewModel.Miembro = miembro;

            return View(miembroViewModel);
        }

        // POST: Miembros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MiembroViewModel miembroViewModel)
        {
            MiembroDto miembroDto = miembroViewModel.Miembro;
            if (ModelState.IsValid)
            {
                try
                {
                    await _miembroService.CreateMiembroAsync(miembroDto);
                    TempData["SuccessMessage"] = "Miembro creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error al crear miembro");
                }
            }

            return View(miembroDto);
        }

        // GET: Miembros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var miembro = await _miembroService.GetMiembroByIdAsync(id);

            if (miembro == null)
            {
                return NotFound();
            }

            MiembroViewModel miembroViewModel = new MiembroViewModel();
            miembroViewModel.Miembro = miembro;

            return View(miembroViewModel);
        }

        // POST: Miembros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MiembroViewModel miembroViewModel)
        {
            MiembroDto miembroDto = miembroViewModel.Miembro;
            if (id != miembroDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _miembroService.UpdateMiembroAsync(miembroDto);
                    TempData["SuccessMessage"] = "Miembro actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Error al actualizar miembro");
                }
            }

            return View(miembroDto);
        }

        // GET: Miembros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var miembro = await _miembroService.GetMiembroByIdAsync(id);

            if (miembro == null)
            {
                return NotFound();
            }

            MiembroViewModel miembroViewModel = new MiembroViewModel();
            miembroViewModel.Miembro = miembro;

            return View(miembroViewModel);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _miembroService.DeleteMiembroAsync(id);
                TempData["SuccessMessage"] = "Miembro eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Error al eliminar miembro");
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
