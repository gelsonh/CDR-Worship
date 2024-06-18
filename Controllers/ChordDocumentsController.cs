using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using System.Net.Sockets;

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
        public async Task<IActionResult> AddChordAttachment([Bind("Id,FormFile,Description,ChordDocumentId, SongName, ChordId")] ChordAttachment chordAttachment)
        {
            string statusMessage;

            if (ModelState.IsValid && chordAttachment.FormFile != null)
            {
                // Verificar si ChordDocumentId es un valor temporal
                if (chordAttachment.ChordDocumentId <= 0)
                {
                    // Crear un nuevo documento de acorde en la base de datos
                    var newChordDocument = new ChordDocument();
                    // Asignar el nombre de la canción si no es nulo o vacío
                    if (!string.IsNullOrEmpty(chordAttachment.MusicName))
                    {
                        newChordDocument.SongName = chordAttachment.MusicName;
                    }
                    else
                    {
                        // Si el nombre de la canción es nulo o vacío, proporciona un valor predeterminado
                        newChordDocument.SongName = string.Empty;
                    }
                    newChordDocument.Description = string.Empty;
                    _context.ChordDocuments.Add(newChordDocument);
                    await _context.SaveChangesAsync();

                    // Asignar el ID del nuevo documento de acorde a ChordDocumentId
                    chordAttachment.ChordDocumentId = newChordDocument.Id;
                }


                // Proceder con el almacenamiento del archivo adjunto
                chordAttachment.FileData = await _fileService.ConvertFileToByteArrayAsync(chordAttachment.FormFile);
                chordAttachment.FileName = chordAttachment.FormFile.FileName;
                chordAttachment.FileType = chordAttachment.FormFile.ContentType;

                chordAttachment.Created = DateTimeOffset.Now.UtcDateTime;
                chordAttachment.AppUserId = _userManager.GetUserId(User);

                // Guardar el archivo adjunto en la base de datos
                await _chordDocumentService.AddChordAttachmentAsync(chordAttachment);
                statusMessage = "Success: New attachment added to the table.";
            }
            else
            {
                statusMessage = "Error: Invalid data.";
            }

            // Redirigir a la acción "Details" con el ID del documento de acorde
            return RedirectToAction("Index", new { id = chordAttachment.ChordDocumentId, message = statusMessage });
        }


        public async Task<IActionResult> ShowFile(int id)
        {
            // Obtener el archivo adjunto del documento de acorde por su ID
            ChordAttachment chordDocumentAttachment = await _chordDocumentService.GetChordAttachmentByIdAsync(id);

            // Verificar si el archivo adjunto existe
            if (chordDocumentAttachment != null && chordDocumentAttachment.FileData != null)
            {
                // Obtener el nombre del archivo adjunto
                string fileName = chordDocumentAttachment.FileName!;

                // Obtener la extensión del archivo
                string ext = Path.GetExtension(fileName).Replace(".", "");

                // Devolver el archivo al cliente
                return File(chordDocumentAttachment.FileData, $"application/{ext}");
            }

            // Devolver una respuesta de error si el archivo adjunto no existe o no se pudo cargar
            return NotFound();
        }


        public async Task<IActionResult> Download(int id)
        {
            var chordDocument = await _chordDocumentService.GetChordDocumentByIdAsync(id);
            if (chordDocument != null && chordDocument.File != null)
            {
                // Devuelve el archivo PDF como un archivo para su descarga
                return File(chordDocument.File, "application/pdf", "nombre-del-documento.pdf");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> ViewDocument(int id)
        {
            var chordDocument = await _chordDocumentService.GetChordDocumentByIdAsync(id);

            if (chordDocument != null && chordDocument.File != null)
            {
                return File(chordDocument.File, "application/pdf"); // Ajusta el tipo MIME según tu necesidad
            }

            return NotFound();
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

                // Verificar si el modelo es válido
                if (ModelState.IsValid)
                {
                    // Verificar el tamaño del archivo
                    if (FormFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await FormFile.CopyToAsync(memoryStream);
                            chordDocument.File = memoryStream.ToArray();
                        }
                    }

                    // Crear un nuevo objeto ChordAttachment
                    var chordAttachment = new ChordAttachment();

                    // Establecer los datos del archivo adjunto
                    chordAttachment.FileName = FormFile.FileName;
                    chordAttachment.FileData = chordDocument.File;
                    chordAttachment.Created = DateTimeOffset.Now.UtcDateTime;
                    chordAttachment.AppUserId = _userManager.GetUserId(User);

                    // Asignar el ChordDocument al ChordAttachment
                    chordAttachment.ChordDocument = chordDocument;

                    // Agregar ambos al contexto
                    _context.Add(chordAttachment);
                    _context.Add(chordDocument);

                    // Guardar los cambios en el contexto
                    await _context.SaveChangesAsync();

                    // Agregar puntos de verificación para los archivos adjuntos
                    foreach (var attachment in chordDocument.ChordAttachments)
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
                return View(chordDocument);
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                ModelState.AddModelError(string.Empty, "An error occurred while saving the document.");
                // Agregar mensajes de registro para excepciones
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

            // Obtener el documento de acorde desde tu contexto
            var chordDocument = await _context.ChordDocuments.FindAsync(id);
            if (chordDocument == null)
            {
                return NotFound();
            }

            // Obtener una lista de los acordes disponibles
            var chords = await _chordDocumentService.GetUniqueChordsAsync();

            // Crear una lista desplegable con los acordes
            ViewBag.ChordId = new SelectList(chords, "Id", "ChordName");

            // Pasar el documento de acorde a tu vista
            return View(chordDocument);
        }



        // POST: ChordDocuments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SongName,Description,Created,Updated,SongDocumentId, ChordId")] ChordDocument chordDocument)
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
            return View(chordDocument);
        }

        // GET: ChordDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chordDocument = await _context.ChordDocuments
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
