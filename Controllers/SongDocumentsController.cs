using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using CDR_Worship.Services;
using Microsoft.AspNetCore.Identity;

namespace CDR_Worship.Controllers
{
    public class SongDocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISongDocumentService _songDocumentService;
        private readonly IFileService _fileService;


        public SongDocumentsController(ApplicationDbContext context, UserManager<AppUser> userManager, ISongDocumentService songDocumentService, IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _songDocumentService = songDocumentService;
            _fileService = fileService;
        }

        // GET: SongDocuments
        public async Task<IActionResult> Index()
        {
            try
            {
                var songDocument = await _songDocumentService.GetAllSongDocumentsAsync();

                var sortedSongDocument = songDocument.OrderBy(cd => cd.SongName);


                return View(sortedSongDocument);
            }

            catch (Exception ex)
            {
                throw new Exception("Error getting all SongDocuments", ex);
            }
        }

        public async Task<IActionResult> Download(int id)
        {
            var songDocument = await _songDocumentService.GetSongDocumentByIdAsync(id);

            if (songDocument != null && songDocument.FileData != null && songDocument.FileName != null)
            {
                // Obtener el tipo de contenido adecuado para el archivo según su extensión
                string ext = Path.GetExtension(songDocument.FileName).ToLowerInvariant();
                string contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".txt" => "text/plain",
                    ".doc" => "application/msword",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".xls" => "application/vnd.ms-excel",
                    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    _ => "application/octet-stream" // Tipo genérico
                };

                // Retornar el archivo para su descarga
                return File(songDocument.FileData, contentType, songDocument.FileName);
            }

            return NotFound();
        }

        public async Task<IActionResult> ViewDocument(int id)
        {
            var songDocument = await _songDocumentService.GetSongDocumentByIdAsync(id);

            if (songDocument != null && songDocument.FileData != null)
            {
                // Ensure the file name is not null or empty
                if (string.IsNullOrEmpty(songDocument.FileName))
                {
                    return BadRequest("File name not found.");
                }

                // Get the file extension and determine the MIME type
                string ext = Path.GetExtension(songDocument.FileName!).ToLowerInvariant();

                // MIME type mapping based on file extension
                string contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".txt" => "text/plain",
                    ".doc" => "application/msword",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".xls" => "application/vnd.ms-excel",
                    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    ".mp3" => "audio/mpeg",
                    ".mp4" => "video/mp4",
                    _ => "application/octet-stream"
                };

                // Ensure the browser tries to display the file inline rather than download it
                Response.Headers["Content-Disposition"] = $"inline; filename=\"{songDocument.FileName}\"";

                // Return the file as a response with the appropriate content type
                return File(songDocument.FileData, contentType);
            }

            // If no document or file data is found, return NotFound
            return NotFound("Document not found or missing file data.");
        }
        // GET: SongDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el ChordDocument correspondiente al id proporcionado
            SongDocument? songDocument = await _songDocumentService.GetSongDocumentByIdAsync(id);

            if (songDocument == null)
            {
                return NotFound();
            }

            return View(songDocument);
        }

        // GET: SongDocuments/Create
        public IActionResult Create()
        {
            // Crear un nuevo objeto ChordDocument y pasarlo a la vista
            var songDocument = new SongDocument(); // Puedes inicializarlo como lo necesites
            return View(songDocument);
        }

        // POST: SongDocuments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile FormFile, [Bind("Id,SongName,Description,Created,Updated,ChordDocumentId")] SongDocument songDocument)
        {
            try
            {
                // Verificar si se recibió el archivo adjunto del formulario
                if (FormFile == null || FormFile.Length == 0)
                {
                    ModelState.AddModelError("FormFile", "Please select a file to upload.");
                    return View(songDocument);
                }

                // Verificar si el modelo es válido
                if (ModelState.IsValid)
                {
                    // Procesar el archivo adjunto y almacenarlo en SongDocument
                    using (var memoryStream = new MemoryStream())
                    {
                        await FormFile.CopyToAsync(memoryStream);
                        songDocument.FileData = memoryStream.ToArray(); // Almacena los bytes del archivo
                        songDocument.FileName = FormFile.FileName; // Almacena el nombre del archivo
                        songDocument.FileType = FormFile.ContentType; // Almacena el tipo MIME del archivo
                    }

                    // Guardar el documento en la base de datos
                    _context.Add(songDocument);
                    await _context.SaveChangesAsync();

                    // Redirigir a la vista de índice
                    return RedirectToAction("Index");
                }

                return View(songDocument);
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                ModelState.AddModelError(string.Empty, "An error occurred while saving the document.");
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View(songDocument);
            }
        }


        // GET: SongDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songDocument = await _context.SongDocuments.FindAsync(id);
            if (songDocument == null)
            {
                return NotFound();
            }
            return View(songDocument);
        }

        // POST: SongDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SongName,Description,Created,Updated,ChordDocumentId")] SongDocument songDocument)
        {
            if (id != songDocument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongDocumentExists(songDocument.Id))
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
            return View(songDocument);
        }

        // GET: SongDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songDocument = await _context.SongDocuments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songDocument == null)
            {
                return NotFound();
            }

            return View(songDocument);
        }

        // POST: SongDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songDocument = await _context.SongDocuments.FindAsync(id);
            if (songDocument != null)
            {
                _context.SongDocuments.Remove(songDocument);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongDocumentExists(int id)
        {
            return _context.SongDocuments.Any(e => e.Id == id);
        }
    }
}
