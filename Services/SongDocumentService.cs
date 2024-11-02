using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Services
{
    public class SongDocumentService : ISongDocumentService
    {

        private readonly ApplicationDbContext _context;
        public SongDocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSongAttachmentAsync(SongAttachment? SongAttachment)
        {
            try
            {
                await _context.AddAsync(SongAttachment!);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding ChordAttachment", ex);
            }
        }


        public async Task<SongAttachment?> GetSongAttachmentByIdAsync(int? songAttachmentId)
        {
            try
            {
                SongAttachment? songAttachment = await _context.SongAttachments
                    .Include(t => t.Song)
                    .Include(t => t.AppUser)
                    .FirstOrDefaultAsync(c => c.Id == songAttachmentId);

                return songAttachment;
            }

            catch (Exception ex)
            {
                throw new Exception("Error getting ChordAttachment", ex);
            }
        }

        public async Task<SongDocument?> GetSongDocumentByIdAsync(int? songDocumentId)
        {
            try
            {
                if (songDocumentId == null)
                {
                    return null;
                }

                SongDocument? songDocument = await _context.SongDocuments

                    .Include(t => t.SongAttachments)
                    .FirstOrDefaultAsync(t => t.Id == songDocumentId);

                return songDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SongDocument>> GetAllSongDocumentsAsync()
        {
            try
            {
                var songDocuments = await _context.SongDocuments
                    .Include(cd => cd.SongAttachments)
                    .ToListAsync();

                return songDocuments;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario (registrándola, lanzándola nuevamente, etc.)
                throw new Exception("Error al obtener todos los documentos.", ex);
            }
        }


    }
}
