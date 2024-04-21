using CDR_Worship.Models;

namespace CDR_Worship.Services.Interfaces
{
    public interface IChordDocumentService
    {
        public Task<IEnumerable<Chord>> GetUniqueChordsAsync();

        public Task AddChordAttachmentAsync(ChordAttachment? ChordAttachment);

        public Task<ChordAttachment?> GetChordAttachmentByIdAsync(int? chordAttachmentId);
        public Task<ChordDocument?> GetChordDocumentByIdAsync(int? chordDocumentId);
        public Task<IEnumerable<ChordDocument>> GetAllChordDocumentsAsync();
        
    }
}
