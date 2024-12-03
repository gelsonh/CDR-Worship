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
        public async Task<IActionResult> Index()
        {
            try
            {
                var chordDocuments = await _chordDocumentService.GetAllChordDocumentsAsync();
                return View(chordDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chord documents.");
                TempData["ErrorMessage"] = "An error occurred while loading the documents.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChordAttachment(IFormFile formFile, ChordAttachment chordAttachment)
        {
            if (!ModelState.IsValid)
            {
                return View(chordAttachment);
            }
            var result = await _chordDocumentService.AddChordAttachmentAsync(formFile, chordAttachment, _userManager.GetUserId(User));

            if (result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return View(chordAttachment);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ShowFile(int id)
        {
            _logger.LogInformation("Intentando mostrar archivo con ID: {Id}", id);

            var fileResult = await _chordDocumentService.GetAttachmentFileAsync(id);

            if (fileResult.HasValue)
            {
                _logger.LogInformation("Archivo con ID {Id} encontrado. Mostrándolo al usuario.", id);
                (byte[]? fileData, string contentType, string fileName) = fileResult.Value;
                Response.Headers.Append("Content-Disposition", $"inline; filename={fileName}");
                return File(fileData!, contentType);
            }

            _logger.LogWarning("Archivo no encontrado para el ID: {Id}", id);
            return NotFound("File not found.");
        }


        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID no válido para la descarga: {Id}", id);
                TempData["ErrorMessage"] = "ID no válido para la descarga.";
                return RedirectToAction("Index");
            }

            try
            {
                // Obtener el archivo usando el servicio
                var fileResult = await _chordDocumentService.GetAttachmentFileAsync(id);

                // Verifica si el archivo existe
                if (fileResult.HasValue)
                {
                    var (fileData, contentType, fileName) = fileResult.Value;

                    if (fileData != null && !string.IsNullOrEmpty(contentType) && !string.IsNullOrEmpty(fileName))
                    {
                        _logger.LogInformation("Archivo con ID {Id} encontrado y descargado.", id);
                        return File(fileData, contentType, fileName);
                    }
                }

                // Archivo no encontrado
                _logger.LogWarning("Archivo no encontrado para el ID: {Id}", id);
                TempData["ErrorMessage"] = "El archivo no se encontró o no está disponible.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Registrar el error
                _logger.LogError(ex, "Error al intentar descargar el archivo con ID: {Id}", id);
                TempData["ErrorMessage"] = "Hubo un error al intentar descargar el archivo. Intenta nuevamente.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> ViewDocument(int id)
        {
            _logger.LogInformation("Intentando visualizar documento con ID: {Id}", id);

            // Obtener el archivo relacionado con el ID
            var fileResult = await _chordDocumentService.GetAttachmentFileAsync(id);

            // Verificar si el archivo existe
            if (fileResult.HasValue)
            {
                var (fileData, contentType, fileName) = fileResult.Value;

                if (fileData != null && !string.IsNullOrEmpty(contentType) && !string.IsNullOrEmpty(fileName))
                {
                    _logger.LogInformation("Archivo con ID {Id} encontrado. Mostrándolo.", id);

                    // Configurar el encabezado para mostrar el archivo en el navegador
                    Response.Headers["Content-Disposition"] = $"inline; filename=\"{fileName}\"";

                    // Retornar el archivo con el tipo de contenido correcto
                    return File(fileData, contentType);
                }
            }

            // Si no se encuentra el archivo, registrar advertencia y redirigir
            _logger.LogWarning("Archivo no encontrado para el ID: {Id}", id);
            TempData["ErrorMessage"] = "El archivo no se encontró o no está disponible.";
            return RedirectToAction("Index");
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
            try
            {
                var chords = await _chordDocumentService.GetUniqueChordsAsync();

                if (!chords.Any())
                {
                    TempData["ErrorMessage"] = "No hay acordes disponibles.";
                    return RedirectToAction("Index");
                }

                ViewBag.ChordId = new SelectList(chords, "Id", "ChordName");
                return View(new ChordDocument());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la vista de creación.");
                TempData["ErrorMessage"] = "Se produjo un error al cargar el formulario.";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile formFile, [Bind("SongName,Description,ChordId")] ChordDocument chordDocument)
        {
            try
            {
                // Validar ModelState
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState no es válido. Errores: {Errors}",
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    await PopulateChordsDropdownAsync();
                    return View(chordDocument);
                }

                // Validar archivo subido
                if (formFile == null || formFile.Length == 0)
                {
                    _logger.LogWarning("No se seleccionó un archivo válido.");
                    ModelState.AddModelError("formFile", "Por favor, selecciona un archivo válido.");
                    await PopulateChordsDropdownAsync();
                    return View(chordDocument);
                }

                // Obtener usuario actual
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Usuario no autenticado al intentar subir un documento.");
                    TempData["ErrorMessage"] = "Debes iniciar sesión para subir un documento.";
                    return RedirectToAction("Login", "Account");
                }

                // Guardar documento
                var result = await _chordDocumentService.AddChordDocumentAsync(formFile, chordDocument, userId);
                if (!result.Success)
                {
                    _logger.LogWarning("Error al guardar el documento: {ErrorMessage}", result.ErrorMessage);
                    ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                    await PopulateChordsDropdownAsync();
                    return View(chordDocument);
                }

                _logger.LogInformation("Documento guardado correctamente con ID: {Id}", chordDocument.Id);
                TempData["SuccessMessage"] = "Documento guardado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la creación del documento.");
                TempData["ErrorMessage"] = "Se produjo un error inesperado. Intenta nuevamente.";
                await PopulateChordsDropdownAsync();
                return View(chordDocument);
            }
        }

        // Método auxiliar para evitar duplicación
        private async Task PopulateChordsDropdownAsync()
        {
            var chords = await _chordDocumentService.GetUniqueChordsAsync();
            ViewBag.ChordId = new SelectList(chords, "Id", "ChordName");
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
