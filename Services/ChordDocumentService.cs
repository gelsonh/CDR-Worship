using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Models.Enums;

namespace CDR_Worship.Services
{
    public class ChordDocumentService : IChordDocumentService
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IChordMappingService _chordMappingService;
        private readonly IFileService _fileService;
        private readonly ILogger<ChordDocumentService> _logger;
        public ChordDocumentService(ApplicationDbContext context, UserManager<AppUser> userManager, IChordMappingService chordMappingService, IFileService fileService, ILogger<ChordDocumentService> logger)
        {
            _context = context;
            _userManager = userManager;
            _chordMappingService = chordMappingService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<(bool Success, string? ErrorMessage)> AddChordDocumentAsync(IFormFile formFile, ChordDocument chordDocument, string? userId)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return (false, "Por favor, selecciona un archivo válido.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Intento de guardar sin usuario autenticado.");
                return (false, "El usuario no está autenticado. Por favor, inicia sesión e inténtalo de nuevo.");
            }

            try
            {
                var fileData = await _fileService.ConvertFileToByteArrayAsync(formFile);

                chordDocument.FileData = fileData;
                chordDocument.FileName = formFile.FileName;
                chordDocument.FileType = formFile.ContentType;
                chordDocument.Created = DateTime.UtcNow;

                var chordAttachment = new ChordAttachment
                {
                    FileName = formFile.FileName,
                    FileData = fileData,
                    FileType = formFile.ContentType,
                    Created = DateTime.UtcNow,
                    AppUserId = userId,
                    ChordDocument = chordDocument
                };

                _context.ChordDocuments.Add(chordDocument);
                _context.ChordAttachments.Add(chordAttachment);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el documento y su archivo adjunto.");
                return (false, "Hubo un error al guardar el archivo.");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> AddChordAttachmentAsync(IFormFile formFile, ChordAttachment chordAttachment, string? userId)
        {
            if (formFile == null || formFile.Length == 0)
                return (false, "El archivo es obligatorio.");

            // Validar extensiones permitidas
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return (false, "Este tipo de archivo no está permitido.");

            try
            {
                // Convertir archivo a byte[]
                chordAttachment.FileData = await _fileService.ConvertFileToByteArrayAsync(formFile);
                chordAttachment.FileName = formFile.FileName;
                chordAttachment.FileType = formFile.ContentType;
                chordAttachment.Created = DateTime.UtcNow;
                chordAttachment.AppUserId = userId;

                // Guardar el archivo en ChordAttachments
                _context.ChordAttachments.Add(chordAttachment);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el archivo adjunto.");
                return (false, "Ocurrió un error al guardar el archivo adjunto.");
            }
        }

        public async Task<IEnumerable<Chord>> GetUniqueChordsAsync()
        {
            var chords = await _context.Chords
                .AsNoTracking() // Mejora el rendimiento
                .GroupBy(ch => ch.ChordName) // Agrupar en la base de datos
                .Select(g => g.First()) // Seleccionar el primer acorde de cada grupo
                .ToListAsync();

            return chords;
        }



        public async Task<ChordAttachment?> GetChordAttachmentByIdAsync(int? chordAttachmentId)
        {
            try
            {
                return await _context.ChordAttachments
                    .Include(ca => ca.ChordDocument)
                    .Include(ca => ca.AppUser)
                    .FirstOrDefaultAsync(c => c.Id == chordAttachmentId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting ChordAttachment", ex);
            }
        }

        public async Task<ChordDocument?> GetChordDocumentByIdAsync(int chordDocumentId)
        {
            try
            {
                ChordDocument? chordDocument = await _context.ChordDocuments
                    .Include(cd => cd.ChordAttachments) // Include attachments
                    .FirstOrDefaultAsync(cd => cd.Id == chordDocumentId);

                return chordDocument;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving ChordDocument", ex);
            }
        }

        public async Task<ChordDocument?> GetChordDocumentByIdAsync(int? chordDocumentId)
        {
            try
            {
                if (chordDocumentId == null)
                {
                    return null;
                }

                ChordDocument? chordDocument = await _context.ChordDocuments
                    .Include(t => t.Chord)
                    .Include(t => t.ChordAttachments)
                    .FirstOrDefaultAsync(t => t.Id == chordDocumentId);

                return chordDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ChordDocument>> GetAllChordDocumentsAsync()
        {
            try
            {
                // Obtener documentos de acordes con datos relacionados
                var chordDocuments = await _context.ChordDocuments
                    .AsNoTracking()
                    .Include(cd => cd.Chord)
                    .ToListAsync();

                // Proyectar datos procesados
                var processedChordDocuments = chordDocuments.Select(chordDocument =>
                {
                    // Verificar que el acorde no sea nulo y tenga un nombre
                    if (chordDocument.Chord != null && !string.IsNullOrEmpty(chordDocument.Chord.ChordName))
                    {
                        try
                        {
                            // Mapear el nombre del acorde
                            chordDocument.Chord.ChordName = _chordMappingService.MapChordName(chordDocument.Chord.ChordName);
                        }
                        catch (ArgumentException ex)
                        {
                            // Registrar advertencia para nombres de acordes inválidos
                            _logger.LogWarning(ex, "Nombre de acorde inválido detectado: {ChordName}", chordDocument.Chord.ChordName);
                        }
                    }

                    return chordDocument;
                });

                return processedChordDocuments;
            }
            catch (Exception ex)
            {
                // Registrar el error y volver a lanzar la excepción
                _logger.LogError(ex, "Error al obtener documentos de acordes.");
                throw new Exception("Error al recuperar documentos de acordes.", ex);
            }
        }


        public async Task<ChordAttachment?> GetChordAttachmentByIdAsync(int chordAttachmentId)
        {

            try
            {
                ChordAttachment? chordAttachment = await _context.ChordAttachments
                    .Include(ca => ca.ChordDocument) // Incluir datos del documento de acorde asociado
                    .Include(ca => ca.AppUser)
                    .FirstOrDefaultAsync(c => c.Id == chordAttachmentId);

                return chordAttachment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ChordAttachment with ID {Id}", chordAttachmentId);
                throw;
            }
        }

        public async Task<(byte[]? FileData, string ContentType, string FileName)?> GetAttachmentFileAsync(int id)
        {
            try
            {
                // Buscar en ChordAttachments el archivo relacionado con el ID proporcionado
                var chordAttachment = await _context.ChordAttachments
                    .AsNoTracking() // Mejora el rendimiento al evitar el seguimiento
                    .FirstOrDefaultAsync(ca => ca.ChordDocumentId == id); // Cambiado a ChordDocumentId

                if (chordAttachment != null)
                {
                    return (chordAttachment.FileData, chordAttachment.FileType!, chordAttachment.FileName!);
                }

                // Log cuando no se encuentra el archivo
                _logger.LogWarning("No se encontró un registro en ChordAttachments para el ID: {Id}", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el archivo con ID: {Id}", id);
                throw;
            }
        }
    }
}
