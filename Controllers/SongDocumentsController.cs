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
        

        public SongDocumentsController(ApplicationDbContext context,UserManager<AppUser> userManager, ISongDocumentService songDocumentService,IFileService fileService)
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
                var songDocuments = await _songDocumentService.GetAllSongDocumentsAsync();

                var sortedSongDocuments = songDocuments.OrderBy(cd => cd.SongName);


                return View(sortedSongDocuments);
            }

            catch (Exception ex)
            {
                throw new Exception("Error getting all SongDocuments", ex);
            }
        }

        public async Task<IActionResult> Download(int id)
        {
            var songDocument = await _songDocumentService.GetSongDocumentByIdAsync(id);
            if (songDocument != null && songDocument.File != null)
            {
                // Devuelve el archivo PDF como un archivo para su descarga
                return File(songDocument.File, "application/pdf", "nombre-del-documento.pdf");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> ViewDocument(int id)
        {
            var songDocument = await _songDocumentService.GetSongDocumentByIdAsync(id);

            if (songDocument != null && songDocument.File != null)
            {
                return File(songDocument.File, "application/pdf"); // Ajusta el tipo MIME según tu necesidad
            }

            return NotFound();
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
                    // Verificar el tamaño del archivo
                    if (FormFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await FormFile.CopyToAsync(memoryStream);
                            songDocument.File = memoryStream.ToArray();
                        }
                    }

                    // Crear un nuevo objeto ChordAttachment
                    var songAttachment = new SongAttachment();

                    // Establecer los datos del archivo adjunto
                    songAttachment.FileName = FormFile.FileName;
                    songAttachment.FileData = songDocument.File;
                    songAttachment.Created = DateTimeOffset.Now.UtcDateTime;
                    songAttachment.AppUserId = _userManager.GetUserId(User);

                    // Asignar el SongDocument al SongAttachment
                    songAttachment.Song = songDocument;

                    // Agregar ambos al contexto
                    _context.Add(songAttachment);
                    _context.Add(songDocument);

                    // Guardar los cambios en el contexto
                    await _context.SaveChangesAsync();

                    // Agregar puntos de verificación para los archivos adjuntos
                    foreach (var attachment in songDocument.SongAttachments)
                    {
                        // Verificar si el archivo adjunto se guardó correctamente en la base de datos
                        if (attachment.Id <= 0)
                        {
                            // Agregar mensajes de registro para verificar los archivos adjuntos
                            Console.WriteLine($"Attachment '{attachment.FileName}' saved successfully.");
                        }
                        else
                        {
                            // Agregar mensajes de registro para errores al guardar archivos adjuntos
                            Console.WriteLine($"Error saving attachment '{attachment.FileName}'.");
                        }
                    }

                    return RedirectToAction("Index");
                }
                return View(songDocument);
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                ModelState.AddModelError(string.Empty, "An error occurred while saving the document.");
                // Agregar mensajes de registro para excepciones
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
