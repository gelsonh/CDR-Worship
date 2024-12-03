using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CDR_Worship.Controllers
{
    public class ChordDocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IChordDocumentService _chordDocumentService;
        private readonly IFileService _fileService;
        private readonly IChordMappingService _chordMappingService;

        private readonly ILogger<ChordDocumentsController> _logger;


        public ChordDocumentsController(ApplicationDbContext context, UserManager<AppUser> userManager, IChordDocumentService chordDocumentService, IFileService fileService, IChordMappingService chordMappingService, ILogger<ChordDocumentsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _chordDocumentService = chordDocumentService;
            _fileService = fileService;
            _chordMappingService = chordMappingService;
            _logger = logger;
        }


        // GET: ChordDocuments
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Obtener todos los documentos de acorde
            var chordDocuments = await _chordDocumentService.GetAllChordDocumentsAsync();

            // Ordenar los documentos de acorde por nombre de la canción
            var sortedChordDocuments = chordDocuments.OrderBy(cd => cd.SongName);

            // Pasar los documentos de acorde ordenados a la vista
            return View(sortedChordDocuments);
        }


        public async Task<IActionResult> Download(int id)
        {
            var fileResult = await _chordDocumentService.PrepareFileForDownloadAsync(id);

            if (fileResult.HasValue)
            {
                var (fileData, contentType, fileName) = fileResult.Value;
                return File(fileData!, contentType, fileName);
            }

            TempData["ErrorMessage"] = "The file does not exist or could not be found.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewDocument(int id)
        {
            var fileResult = await _chordDocumentService.PrepareFileForViewAsync(id);

            if (fileResult.HasValue)
            {
                var (fileData, contentType) = fileResult.Value;

                // Ensure the browser tries to display the file inline
                Response.Headers["Content-Disposition"] = $"inline; filename=\"file\"";

                return File(fileData!, contentType);
            }

            TempData["ErrorMessage"] = "The file does not exist or could not be found.";
            return RedirectToAction(nameof(Index));
        }

        //GET: ChordDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el ChordDocument correspondiente al id proporcionado
            var chordDocument = await _chordDocumentService.GetChordDocumentByIdAsync(id);

            if (chordDocument == null)
            {
                return NotFound();
            }

            return View(chordDocument);
        }


        // GET: ChordDocuments/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtener una lista de los acordes disponibles
            var chords = await _chordDocumentService.GetUniqueChordsAsync();

            // Crear una lista desplegable con los acordes
            ViewBag.ChordId = new SelectList(chords, "Id", "ChordName");

            // Crear un nuevo objeto ChordDocument y pasarlo a la vista
            var chordDocument = new ChordDocument(); // Puedes inicializarlo como lo necesites
            return View(chordDocument);
        }


        private async Task PopulateChordsDropdownAsync()
        {
            try
            {
                // Obtener los acordes únicos desde el servicio
                var chords = await _chordDocumentService.GetUniqueChordsAsync();

                // Crear una lista desplegable con los acordes
                ViewBag.ChordId = new SelectList(chords, "Id", "ChordName");
            }
            catch (Exception ex)
            {
                // Manejar errores y registrar información de logs si es necesario
                _logger.LogError(ex, "Error al llenar la lista de acordes para el dropdown.");
                ViewBag.ChordId = new SelectList(Enumerable.Empty<SelectListItem>());
                TempData["ErrorMessage"] = "Hubo un error al cargar los acordes disponibles.";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile formFile, [Bind("Id,SongName,Description,Created,Updated,ChordId")] ChordDocument chordDocument)
        {
            if (!ModelState.IsValid)
            {
                await PopulateChordsDropdownAsync();
                return View(chordDocument);
            }

            var userId = _userManager.GetUserId(User);
            var result = await _chordDocumentService.CreateChordDocumentAsync(formFile, chordDocument, userId);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                await PopulateChordsDropdownAsync();
                return View(chordDocument);
            }

            TempData["SuccessMessage"] = "Document and attachment successfully saved.";
            return RedirectToAction("Index");
        }

        // GET: ChordDocuments/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el documento de acorde desde tu contexto, incluyendo la información del acorde
            var chordDocument = await _context.ChordDocuments
                .Include(cd => cd.Chord)
                .FirstOrDefaultAsync(cd => cd.Id == id);

            if (chordDocument == null)
            {
                return NotFound();
            }

            // Obtener una lista de los acordes disponibles
            var chords = await _chordDocumentService.GetUniqueChordsAsync();

            // Crear una lista desplegable con los acordes, usando los nombres correctos (mapeados)
            ViewBag.ChordId = new SelectList(chords, "Id", "ChordName", chordDocument.ChordId);

            // Pasar el documento de acorde a tu vista
            return View(chordDocument);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SongName,Description,Created,Updated,SongDocumentId,ChordId")] ChordDocument chordDocument)
        {
            if (id != chordDocument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chordDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChordDocumentExists(chordDocument.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // En caso de error, vuelve a cargar los acordes para mostrarlos nuevamente
            var chords = await _chordDocumentService.GetUniqueChordsAsync();
            ViewBag.ChordId = new SelectList(chords, "Id", "ChordName", chordDocument.ChordId);

            return View(chordDocument);
        }


        // GET: ChordDocuments/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el documento de acorde con la información del acorde
            var chordDocument = await _context.ChordDocuments
                .Include(cd => cd.Chord) // Incluye los datos de Chord
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chordDocument == null)
            {
                return NotFound();
            }

            return View(chordDocument);
        }


        // POST: ChordDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chordDocument = await _context.ChordDocuments.FindAsync(id);
            if (chordDocument != null)
            {
                _context.ChordDocuments.Remove(chordDocument);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChordDocumentExists(int id)
        {
            return _context.ChordDocuments.Any(e => e.Id == id);
        }
    }
}
