using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Models.ViewModel;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Worship.Controllers
{
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAudioService _audioService;
        private readonly IChordDocumentService _chordDocumentService;

        public SongsController(ApplicationDbContext context, IAudioService audioService, IChordDocumentService chordDocumentService)
        {
            _context = context;
            _audioService = audioService;
            _chordDocumentService = chordDocumentService;
        }


        // GET: Crear una nueva canción
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new SongAudioViewModel
            {
                Chords = await _chordDocumentService.GetUniqueChordsAsync() // Llenar con acordes únicos
            };
            return View(viewModel);
        }

        // POST: Guardar nueva canción
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongAudioViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _audioService.SaveAudioAsync(model);
                return RedirectToAction(nameof(Index));
            }

            // Si hay un error, recargar la lista de acordes
            model.Chords = _context.Chords.ToList();
            return View(model);
        }

        // GET: Editar una canción
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var songAudio = await _audioService.GetSongAudioByIdAsync(id);
            if (songAudio == null)
            {
                return NotFound();
            }

            var viewModel = new SongAudioViewModel
            {
                Id = songAudio.Id,
                SongName = songAudio.SongName,
                YouTubeUrl = songAudio.YouTubeUrl,
                ChordId = songAudio.ChordId,
                Chords = await _chordDocumentService.GetUniqueChordsAsync() // Llenar con acordes únicos
            };

            return View(viewModel);
        }

        // POST: Guardar cambios de una canción existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SongAudioViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _audioService.UpdateAudioAsync(model);
                return RedirectToAction(nameof(Index));
            }

            model.Chords = _context.Chords.ToList(); // Recargar la lista de acordes si hay error
            return View(model);
        }

        // POST: Eliminar canción existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _audioService.DeleteAudioAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Mostrar todas las canciones (Index)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var songs = await _audioService.GetAllSongsAsync();
            return View(songs);
        }
    }
}