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

        public ChordDocumentsController(ApplicationDbContext context, UserManager<AppUser> userManager, IChordDocumentService chordDocumentService, IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _chordDocumentService = chordDocumentService;
            _fileService = fileService;
        }


        // GET: ChordDocuments
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener todos los documentos de acorde
                var chordDocuments = await _chordDocumentService.GetAllChordDocumentsAsync();

                // Ordenar los documentos de acorde por nombre de la canción
                var sortedChordDocuments = chordDocuments.OrderBy(cd => cd.SongName);

                // Pasar los documentos de acorde ordenados a la vista
                return View(sortedChordDocuments);
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario (registrándola, mostrando un mensaje de error, etc.)
                return StatusCode(500, "Error al cargar los documentos de acorde: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChordAttachment(IFormFile FormFile, [Bind("Id, MusicName, Description, ChordDocumentId")] ChordAttachment chordAttachment)
        {
            if (FormFile == null || FormFile.Length == 0)
            {
                ModelState.AddModelError("FormFile", "Please select a file to upload.");
                return View(chordAttachment);
            }

            if (ModelState.IsValid)
            {
                // Convertir el archivo a un array de bytes
                chordAttachment.FileData = await _fileService.ConvertFileToByteArrayAsync(FormFile);

                // Asignar nombre y tipo de archivo
                chordAttachment.FileName = FormFile.FileName;
                chordAttachment.FileType = FormFile.ContentType;

                chordAttachment.Created = DateTime.UtcNow;
                chordAttachment.AppUserId = _userManager.GetUserId(User);

                // Guardar en la base de datos
                _context.Add(chordAttachment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(chordAttachment);
        }

        public async Task<IActionResult> ShowFile(int id)
        {
            // Obtener el archivo adjunto del documento de acorde por su ID
            ChordAttachment? chordDocumentAttachment = await _chordDocumentService.GetChordAttachmentByIdAsync(id);

            // Verificar si el archivo adjunto existe
            if (chordDocumentAttachment != null && chordDocumentAttachment.FileData != null)
            {
                // Obtener el nombre del archivo adjunto
                string fileName = chordDocumentAttachment.FileName!;
                byte[]? fileData = chordDocumentAttachment.FileData;
                // Obtener la extensión del archivo
                string ext = Path.GetExtension(fileName).Replace(".", "");

                Response.Headers.Append("Content-Disposition", $"inline; filename={fileName}");

                // Devolver el archivo al cliente
                return File(chordDocumentAttachment.FileData, $"application/{ext}");
            }

            // Devolver una respuesta de error si el archivo adjunto no existe o no se pudo cargar
            return NotFound();
        }


        public async Task<IActionResult> Download(int id)
        {
            var chordAttachment = await _chordDocumentService.GetChordAttachmentByIdAsync(id);

            if (chordAttachment != null && chordAttachment.FileData != null)
            {
                return File(chordAttachment.FileData, chordAttachment.FileType!, chordAttachment.FileName);
            }

            return NotFound("File not found.");
        }

        public async Task<IActionResult> ViewDocument(int id)
        {
            var chordAttachment = await _chordDocumentService.GetChordAttachmentByIdAsync(id);

            if (chordAttachment != null && chordAttachment.FileData != null)
            {
                // Determinar el tipo de archivo a partir de la extensión
                string ext = Path.GetExtension(chordAttachment.FileName!).ToLowerInvariant();
                string contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".doc" => "application/msword",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    _ => "application/octet-stream"
                };


                // Enviar el archivo al navegador para visualizarlo directamente
                return File(chordAttachment.FileData, contentType);
            }

            return NotFound("File not found.");
        }

        //GET: ChordDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el ChordDocument correspondiente al id proporcionado
            ChordDocument? chordDocument = await _chordDocumentService.GetChordDocumentByIdAsync(id);

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile FormFile, [Bind("Id,SongName,Description,Created,Updated,SongDocumentId, ChordId")] ChordDocument chordDocument)
        {
            try
            {
                // Verificar si se recibió el archivo adjunto del formulario
                if (FormFile == null || FormFile.Length == 0)
                {
                    ModelState.AddModelError("FormFile", "Please select a file to upload.");
                    return View(chordDocument);
                }

                // Validar la extensión del archivo
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx" };
                var ext = Path.GetExtension(FormFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("FormFile", "This file type is not allowed.");
                    return View(chordDocument);
                }

                // Verificar si el modelo es válido
                if (ModelState.IsValid)
                {
                    // Utilizar el servicio de archivos para convertir el archivo en un array de bytes
                    chordDocument.FileData = await _fileService.ConvertFileToByteArrayAsync(FormFile);

                    // Crear un nuevo objeto ChordAttachment para el archivo adjunto
                    var chordAttachment = new ChordAttachment
                    {
                        FileName = FormFile.FileName,           // Nombre del archivo
                        FileData = chordDocument.FileData,      // Los bytes del archivo
                        FileType = FormFile.ContentType,        // Tipo de archivo (MIME)
                        Created = DateTime.UtcNow,              // Fecha de creación
                        AppUserId = _userManager.GetUserId(User),  // Usuario que subió el archivo
                        ChordDocument = chordDocument           // Asociar el documento de acorde
                    };

                    // Guardar el documento y el adjunto en la base de datos
                    _context.Add(chordDocument);
                    _context.Add(chordAttachment);
                    await _context.SaveChangesAsync();

                    // Redirigir a la vista de índice con un mensaje de éxito
                    TempData["SuccessMessage"] = "Document and attachment successfully saved.";
                    return RedirectToAction("Index");
                }

                return View(chordDocument);
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                ModelState.AddModelError(string.Empty, "An error occurred while saving the document.");
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View(chordDocument);
            }
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
