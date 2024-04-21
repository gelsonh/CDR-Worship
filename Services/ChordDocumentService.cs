using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Services
{
    public class ChordDocumentService : IChordDocumentService
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ChordDocumentService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Chord>> GetUniqueChordsAsync()
        {

            try 
            {
               var chordDocuments = await _context.Chords
               .GroupBy(c => c.ChordName)
               .Select(g => g.FirstOrDefault())           
               .ToListAsync();

                return chordDocuments!;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario (registrándola, lanzándola nuevamente, etc.)
                throw new Exception("Error al obtener todos los documentos de acordes.", ex);
            }
        }

      
        public async Task AddChordAttachmentAsync(ChordAttachment? ChordAttachment)
        {
            try
            {
                await _context.AddAsync(ChordAttachment!);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding ChordAttachment", ex);
            }
        }

        public async Task<ChordAttachment?> GetChordAttachmentByIdAsync(int? chordAttachmentId)
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

                return chordDocuments;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario (registrándola, lanzándola nuevamente, etc.)
                throw new Exception("Error al obtener todos los documentos de acordes.", ex);
            }
        }


        public Task<ChordDocument?> GetChordDocumentByIdAsync(int chordDocumentId)
        {
            throw new NotImplementedException();
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
    }
}
