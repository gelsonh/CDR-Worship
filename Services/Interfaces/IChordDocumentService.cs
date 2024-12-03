using CDR_Worship.Models;

namespace CDR_Worship.Services.Interfaces
{
    public interface IChordDocumentService
    {

        Task<(bool Success, string? ErrorMessage)> AddChordDocumentAsync(IFormFile formFile, ChordDocument chordDocument, string? userId);
        public Task<IEnumerable<Chord>> GetUniqueChordsAsync();

        Task<(bool Success, string? ErrorMessage)> AddChordAttachmentAsync(IFormFile formFile, ChordAttachment chordAttachment, string? userId);
        public Task<ChordAttachment?> GetChordAttachmentByIdAsync(int? chordAttachmentId);
        public Task<ChordDocument?> GetChordDocumentByIdAsync(int? chordDocumentId);
        public Task<IEnumerable<ChordDocument>> GetAllChordDocumentsAsync();
        Task<(byte[]? FileData, string ContentType, string FileName)?> GetAttachmentFileAsync(int id);

    }
}
