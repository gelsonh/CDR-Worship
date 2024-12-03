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

        public async Task<IEnumerable<Chord>> GetUniqueChordsAsync()
        {
            var chords = await _context.Chords.ToListAsync();
            return chords.GroupBy(ch => _chordMappingService.MapChordName(ch.ChordName!))
            .Select(g => g.First());
        }


        public async Task AddChordAttachmentAsync(IFormFile formFile, ChordAttachment chordAttachment, string userId)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            chordAttachment.FileData = await _fileService.ConvertFileToByteArrayAsync(formFile);
            chordAttachment.FileName = formFile.FileName;
            chordAttachment.FileType = formFile.ContentType;
            chordAttachment.Created = DateTime.UtcNow;
            chordAttachment.AppUserId = userId;

            _context.Add(chordAttachment);
            await _context.SaveChangesAsync();
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
                // Obtener todos los documentos de acordes incluyendo los datos de los acordes asociados
                var chordDocuments = await _context.ChordDocuments
                    .Include(cd => cd.ChordAttachments)
                    .Include(cd => cd.Chord)
                    .ToListAsync();

                // Actualizar los nombres de los acordes usando el mapeo para incluir sostenidos (#)
                foreach (var chordDocument in chordDocuments)
                {
                    if (chordDocument.Chord != null && Enum.TryParse(typeof(CDRChord), chordDocument.Chord.ChordName, out var parsedChord))
                    {
                        chordDocument.Chord.ChordName = CDRChordMapper.ChordNames[(CDRChord)parsedChord];
                    }
                    else
                    {
                        // En caso de que el parseo falle, puedes agregar una acción alternativa, como:
                        // Console.WriteLine($"Advertencia: No se pudo mapear el acorde: {chordDocument.Chord?.ChordName}");
                    }
                }

                return chordDocuments;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario (registrándola, lanzándola nuevamente, etc.)
                throw new Exception("Error al obtener todos los documentos de acordes.", ex);
            }
        }

        public async Task<(byte[]? FileData, string ContentType, string FileName)?> PrepareFileForDownloadAsync(int id)
        {
            var chordDocument = await GetChordDocumentByIdAsync(id);

            if (chordDocument != null && chordDocument.FileData != null && chordDocument.FileName != null)
            {
                string ext = Path.GetExtension(chordDocument.FileName).ToLowerInvariant();
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
                    _ => "application/octet-stream"
                };

                return (chordDocument.FileData, contentType, chordDocument.FileName);
            }

            return null;
        }

        public async Task<(byte[]? FileData, string ContentType)?> PrepareFileForViewAsync(int id)
        {
            var chordDocument = await GetChordDocumentByIdAsync(id);

            if (chordDocument != null && chordDocument.FileData != null)
            {
                string ext = Path.GetExtension(chordDocument.FileName!).ToLowerInvariant();
                string contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".doc" => "application/msword",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    _ => "application/octet-stream"
                };

                return (chordDocument.FileData, contentType);
            }

            return null;
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
                throw new Exception("Error getting ChordAttachment", ex);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateChordDocumentAsync(IFormFile formFile, ChordDocument chordDocument, string userId)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return (false, "Please select a valid file.");
            }

            // Validar extensiones permitidas
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx" };
            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                return (false, "This file type is not allowed.");
            }

            try
            {
                // Convertir el archivo a byte array
                var fileData = await _fileService.ConvertFileToByteArrayAsync(formFile);

                // Crear el ChordDocument
                chordDocument.FileData = fileData;
                chordDocument.FileName = formFile.FileName;
                chordDocument.FileType = formFile.ContentType;
                chordDocument.Created = DateTime.UtcNow;

                // Crear el ChordAttachment
                var chordAttachment = new ChordAttachment
                {
                    FileName = formFile.FileName,
                    FileData = fileData,
                    FileType = formFile.ContentType,
                    Created = DateTime.UtcNow,
                    AppUserId = userId,
                    ChordDocument = chordDocument
                };

                // Guardar en la base de datos
                _context.ChordDocuments.Add(chordDocument);
                _context.ChordAttachments.Add(chordAttachment);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving ChordDocument and attachment.");
                return (false, "An error occurred while saving the document.");
            }
        }
    }
}