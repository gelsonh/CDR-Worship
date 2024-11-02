using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Models.Enums; // Espacio de nombres para el enum CDRChord y el mapper

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
                // Obtener todos los acordes de la base de datos
                var chords = await _context.Chords.ToListAsync();

                // Crear un conjunto para almacenar los nombres de los acordes únicos
                var uniqueChords = new Dictionary<string, Chord>();

                foreach (var chord in chords)
                {
                    if (Enum.TryParse(typeof(CDRChord), chord.ChordName, out var parsedChord))
                    {
                        // Obtener el nombre mapeado del acorde
                        var chordName = CDRChordMapper.ChordNames[(CDRChord)parsedChord];

                        // Si el acorde con este nombre aún no está en el diccionario, lo añadimos
                        if (!uniqueChords.ContainsKey(chordName))
                        {
                            chord.ChordName = chordName;
                            uniqueChords[chordName] = chord;
                        }
                    }
                }

                // Retornar solo los acordes únicos
                return uniqueChords.Values;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario (registrándola, lanzándola nuevamente, etc.)
                throw new Exception("Error al obtener los acordes únicos.", ex);
            }
        }

        public async Task AddChordAttachmentAsync(ChordAttachment? chordAttachment)
        {
            try
            {
                await _context.AddAsync(chordAttachment!);
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
                    .Include(ca => ca.ChordDocument) // Include related ChordDocument data
                    .Include(ca => ca.AppUser)      // Include related AppUser data
                    .FirstOrDefaultAsync(c => c.Id == chordAttachmentId);

                return chordAttachment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting ChordAttachment", ex);
            }
        }

        // Method to get the ChordDocument by its Id and retrieve the file data
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
